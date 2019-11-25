using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models.Data;

namespace WebApp.Models.Shop
{
    public class ShopProductVM
    {
        public ShopProductVM() { }
        public ShopProductVM(ShopCategoryDB model)
        {

        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Pice { get; set; }
        public int ShopCategoryId { get; set; }
        public string ShopCategoryName { get; set; }
        public string ImageName { get; set; }
    }
}