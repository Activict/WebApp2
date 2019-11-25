using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Db : DbContext
    {
        public Db() : base("WebAppDb") { }

        //public static WebAppDb Create()
        //{
        //    return new WebAppDb();
        //}
    }
}