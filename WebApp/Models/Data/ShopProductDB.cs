using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApp.Models.Data
{
    [Table("ShopProduct")]
    public class ShopProductDB
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int ShopCategoryId { get; set; }
        public string ShopCategoryName { get; set; }
        public string ImageName { get; set; }
    }
}