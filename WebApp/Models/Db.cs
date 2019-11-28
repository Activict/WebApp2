using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApp.Models.Data;

namespace WebApp.Models
{
    public class Db : DbContext
    {
        public Db() : base("WebAppDb") { }

        public DbSet<ShopCategoryDB> ShopCategories { get; set; }
        public DbSet<ShopProductDB> ShopProducts { get; set; }
        public DbSet<ShopCartDB> ShopCarts { get; set; }

        //public static WebAppDb Create()
        //{
        //    return new WebAppDb();
        //}
    }
}