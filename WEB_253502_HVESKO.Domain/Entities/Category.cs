using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WEB_253502_HVESKO.Domain.Entities
{
    public class Category
    {
        public int ID { get; set; }

        public string Name { get; set; }

        private string normalizedName;

        public string NormalizedName
        {
            get => normalizedName;
            set => normalizedName = ConvertToKebabCase(value);
        }

        private string ConvertToKebabCase(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return string.Empty;

            return Regex.Replace(str.Trim(), @"\s+", "-").ToLower();
        }
    }
}
