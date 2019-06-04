using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AnkiSyncServer.Model
{
    [DataContract]
    public class LoginViewModel
    {
        [DataMember(Name="u")]
        public string Username { get; set; }

        [DataMember(Name="p")]
        public string Password { get; set; }
    }
}
