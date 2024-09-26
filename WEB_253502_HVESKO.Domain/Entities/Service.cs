using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_253502_HVESKO.Domain.Entities
{
    internal class Service
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string? Category { get; set; }

        public decimal Price { get; set; }

        public string? ImagePath { get; set; }

        public string MimeType { get; set; }
    }
}
