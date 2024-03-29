﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class RoleModel : IdentityRole
    {
    }

    class AppRoleManager : RoleManager<RoleModel>
    {
        public AppRoleManager(RoleStore<RoleModel> store) : base(store) { }
        
        public static AppRoleManager Create(IdentityFactoryOptions<AppRoleManager> options, IOwinContext context)
        {
            return new AppRoleManager(new RoleStore<RoleModel>(context.Get<AppContext>()));
        }
    }

}