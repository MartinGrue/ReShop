using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Image
    {
        public string id { get; set; }
        public string url { get; set; }
        public bool isMain { get; set; }
    }
}