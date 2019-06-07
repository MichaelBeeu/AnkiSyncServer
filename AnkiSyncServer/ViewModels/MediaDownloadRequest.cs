using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AnkiSyncServer.ViewModels
{
    [DataContract]
    public class MediaDownloadRequest
    {
        [DataMember(Name="files")]
        public List<string> Files { get; set; }
    }
}
