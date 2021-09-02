﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models
{
    public class ProductCategory
    {
        public string Id { get; set; }

        [DisplayName("Category Name")]
        public string CategoryName { get; set; }

        public ProductCategory()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
