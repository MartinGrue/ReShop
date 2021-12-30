using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Review
    {
        public Guid id { get; set; }
        
        
        public int rating { get; set; }
        public DateTime createdAt { get; set; }
        public string? text { get; set; }

    }
}