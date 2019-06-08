using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AnkiSyncServer.Middleware
{
    public class AnkiMiddleware
    {
        private readonly RequestDelegate _next;

        public AnkiMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            HttpRequest request = context.Request;
            FormFile file = null;

            try
            {
                file = (FormFile)request.Form.Files["data"];
            }
            catch (InvalidOperationException)
            {
                // Do nothing because the file isn't available
                // and that doesn't necessary indicate an error.
                // TODO: Find a better solution for this...
            }

            if (file != null)
            {
                Boolean compression = request.Form["c"] == "1";
                Stream newBody = file.OpenReadStream();

                if (compression)
                {
                    // GZipStream cannot be reused, so I need to copy the data out of it
                    using (GZipStream gzipStream = new GZipStream(newBody, CompressionMode.Decompress))
                    {
                        newBody = new MemoryStream();
                        await gzipStream.CopyToAsync(newBody);
                        newBody.Seek(0, SeekOrigin.Begin);
                    }
                }

                // The client doesn't inform us of the datatype of the data sent.
                // So I need to attempt to decode the data as JSON, and if it fails, then
                // assume the data is binary.
                try
                {
                    using (var reader = new StreamReader(newBody,
                        Encoding.UTF8,
                        false,
                        1024,
                        true
                        ))
                    {
                        JToken.ReadFrom(new JsonTextReader(reader));
                    }

                    request.ContentType = "application/json";
                }
                catch (JsonReaderException jex)
                {
                    Console.WriteLine(jex.Message);
                }
                finally
                {
                    newBody.Seek(0, SeekOrigin.Begin);
                }

                context.Request.Body = newBody;
            }

            await _next(context);
        }
    }
}
