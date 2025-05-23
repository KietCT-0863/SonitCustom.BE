﻿using SonitCustom.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonitCustom.BLL.DTOs
{
    public class ProductDTO
    {
        public string ProId { get; set; }

        public string ProName { get; set; }

        public string Description { get; set; }

        public string ImgUrl { get; set; }

        public string Price { get; set; }

        public string Category { get; set; }
    }
}
