using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class RolesController : Controller
    {
        AppRoleManager RoleManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<AppRoleManager>(); }
        }


        // GET: Roles
        public ActionResult Index()
        {
            return View(RoleManager.Roles);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await RoleManager.CreateAsync(model);

                if (result.Succeeded)
                {
                    RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "This role exist");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            RoleModel role = await RoleManager.FindByIdAsync(id);
            if (role != null)
            {
                return View(new RoleModel { Id = role.Id, Name = role.Name });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Edit(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                RoleModel role = await RoleManager.FindByIdAsync(model.Id);
                if (role != null)
                {
                    role.Name = model.Name;
                    IdentityResult result = await RoleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Role does't edited");
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            RoleModel role = await RoleManager.FindByIdAsync(id);
            if (role != null)
            {
                return View(new RoleModel { Id = role.Id, Name = role.Name });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(RoleModel model)
        {
            var role = await RoleManager.DeleteAsync(model);

            if (role == null)
            {
                ModelState.AddModelError("", "Role does't delete");
            }

            return RedirectToAction("Index"); ;
        }

    }
}