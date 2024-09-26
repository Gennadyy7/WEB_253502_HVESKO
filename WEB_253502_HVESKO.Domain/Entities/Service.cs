using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_253502_HVESKO.Domain.Entities
{
    public class Service
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public decimal Price { get; set; }

        public string? ImagePath { get; set; }
    }
}
