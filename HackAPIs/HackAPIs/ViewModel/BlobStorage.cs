using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.ViewModel
{
    public class BlobStorage
    {
        public string Connection { get; set; }
        public string Container { get; set; }
        public string Blob { get; set; }
    }
}
