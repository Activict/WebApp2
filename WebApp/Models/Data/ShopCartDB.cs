using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApp.Models.Data
{
    [Table("ShopCart")]
    public class ShopCartDB
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int AmountProduct { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }

        //[ForeignKey("UserId")]
        //public virtual UserModel user { get; set; }
        [ForeignKey("ProductId")]
        public virtual ShopProductDB Product { get; set; }
    }
}