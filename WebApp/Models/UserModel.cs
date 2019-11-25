using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class UserModel : IdentityUser
    {
    }

    public class AppContext : IdentityDbContext<UserModel>
    {
        public AppContext() : base("WebAppDb") { }
        
        public static AppContext Create()
        {
            return new AppContext();
        }
    }

    public class AppUserManager : UserManager<UserModel>
    {
        public AppUserManager(IUserStore<UserModel> store) : base(store) { }

        public static AppUserManager Create (IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            AppContext db = context.Get<AppContext>();
            AppUserManager manager = new AppUserManager(new UserStore<UserModel>(db));
            return manager;
        }
    }
}