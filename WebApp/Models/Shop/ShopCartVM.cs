using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApp.Models.Data;

namespace WebApp.Models.Shop
{
    public class ShopCartVM
    {
        public ShopCartVM() { }
        public ShopCartVM(ShopCartDB model)
        {
            Id = model.Id;
            UserId = model.UserId;
            ProductId = model.ProductId;
            ProductName = model.ProductName;
            Description = model.Description;
            AmountProduct = model.AmountProduct;
            Price = model.Price;
            Image = model.Image;
        }

        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        [DisplayName("Product")]
        public string ProductName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [DisplayName("Amount")]
        public int AmountProduct { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string Image { get; set; }

        public decimal Total { get { return Price * AmountProduct; } }
    }
}