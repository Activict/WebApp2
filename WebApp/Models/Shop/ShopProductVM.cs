using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models.Data;

namespace WebApp.Models.Shop
{
    public class ShopProductVM
    {
        public ShopProductVM() { }
        public ShopProductVM(ShopProductDB model)
        {
            Id = model.Id;
            Name = model.Name;
            Price = model.Price;
            ShopCategoryId = model.ShopCategoryId;
            ShopCategoryName = model.ShopCategoryName;
            ImageName = model.ImageName;

        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int ShopCategoryId { get; set; }
        [DisplayName("Category")]
        public string ShopCategoryName { get; set; }
        [DisplayName("Image")]
        public string ImageName { get; set; }

        public IEnumerable<SelectListItem> ShopCategories { get; set; }
        public IEnumerable<string> GalleryImages { get; set; }
    }
}