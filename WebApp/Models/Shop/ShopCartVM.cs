using System;
using System.Collections.Generic;
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
            AmountProduct = model.AmountProduct;
        }

        [Required]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int AmountProduct { get; set; }
    }
}