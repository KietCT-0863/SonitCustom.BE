using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SonitCustom.BLL.DTOs
{
    public class UpdateProductDTO
    {
        public string ProName { get; set; }

        public string Description { get; set; }

        public string ImgUrl { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "Giá chỉ được chứa số")]
        public string Price { get; set; }

        public string Category { get; set; }
    }
}
