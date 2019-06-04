using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnkiSyncServer.Middleware
{
    public class JwtBodyMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtBodyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            try
            {
                var form = context.Request.Form;
                if (form != null)
                {

                    // If a hostkey is passed, then pull it out of the form data, and move it to a bearer token.
                    string hkey = null;
                    if (form.ContainsKey("k"))
                    {
                        hkey = form["k"];
                    } else if (form.ContainsKey("sk"))
                    {
                        hkey = form["sk"];
                    }

                    if (!String.IsNullOrEmpty(hkey))
                    {
                        context.Request.Headers["Authorization"] = "Bearer " + hkey;
                    }
                }
            } catch (InvalidOperationException)
            {
                // Do nothing...
            }

            return _next(context);
        }
    }
}
