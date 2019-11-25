using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models.Data;

namespace WebApp.Models.Shop
{
    public class ShopCategoryVM
    {
        public ShopCategoryVM() { }
        public ShopCategoryVM(ShopCategoryDB model)
        {
            Id = model.Id;
            Name = model.Name;
            Sorting = model.Sorting;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Sorting { get; set; }

    }
}