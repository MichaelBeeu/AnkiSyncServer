﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AnkiSyncServer.ViewModels
{
    [DataContract]
    public class MediaChanges
    {
        [DataMember(Name = "lastUsn")]
        public long LastUpdateSequenceNumber { get; set; }
    }
}
