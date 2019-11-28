using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.Models.Shop;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        AppUserManager UserManager { get { return HttpContext.GetOwinContext().GetUserManager<AppUserManager>(); } }
        IAuthenticationManager AuthenticationManager { get { return HttpContext.GetOwinContext().Authentication; } }

        public object AuthManager { get; private set; }

        public ActionResult UserMenuPartial()
        {
            string user = User.Identity.Name;

            UserModel userDb = UserManager.Users.FirstOrDefault(m => m.UserName == user);

            return PartialView("_UserMenuPartial", userDb);
        }

        public ActionResult UserProfile()
        {
            string user = User.Identity.Name;
            UserModel userDb = UserManager.Users.FirstOrDefault(m => m.UserName == user);
            if (userDb == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(userDb);
        }

        //[HttpGet]
        //public ActionResult EditProfile()
        //{
        //    string user = User.Identity.Name;
        //    UserModel userDb = UserManager.Users.FirstOrDefault(m => m.UserName == user);
        //    if (userDb == null)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

        //    return View(userDb);
        //}

        //[HttpPost]
        //public ActionResult EditProfile(UserModel user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        UserModel userDb = UserManager.Users.FirstOrDefault(m => m.UserName == user.UserName);

        //        if (userDb != null)
        //        {
        //            if (userDb.UserName != user.UserName || userDb.Email != user.Email || userDb.PhoneNumber != user.PhoneNumber)
        //            {
        //                using (Db db = new Db())
        //                {
        //                    if (db.)
        //                    {

        //                    }
        //                }
        //            }
        //            UserEditVM userEdit = new UserEditVM { UserName = user.UserName, Email = user.Email, PhoneNumber = user.PhoneNumber };
        //        }
        //    }
        //    return RedirectToAction("Index", "Home");
        //}

        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                UserModel user = await UserManager.FindAsync(model.Email, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Incorrect login or password");
                }
                else
                {
                    ClaimsIdentity claim = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = true }, claim);
                    Session["UserId"] = user.Id;

                    if (Session["Cart"] == null)
                    {
                        using (Db db = new Db())
                        {
                            Session["Cart"] = db.ShopCarts.ToArray().Where(m => m.UserId == user.Id).Select(m => new ShopCartVM(m)).ToList();
                        }
                    }

                    if (Session["Cart"] != null)
                        return RedirectToAction("Index", "Home");

                    return Redirect(returnUrl);
                }
            }
            ViewBag.returnUrl = returnUrl;
            return View(model);
        }

        public ActionResult Logout()
        {
            Session["Cart"] = Session["UserId"] = null;
            AuthenticationManager.SignOut();
            return RedirectToAction("Login");
        }

        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                UserModel user = new UserModel { Email = model.Email, UserName = model.Email };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                
                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            UserModel user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(UserModel user)
        {
            if (ModelState.IsValid)
            {
                UserModel userDel = await UserManager.FindByEmailAsync(user.Email);

                if (userDel == null)
                {
                    TempData["Message"] = "User email does't exist";
                    return View();
                }

                IdentityResult result = await UserManager.DeleteAsync(userDel);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {

            UserModel user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Edit(UserModel user) 
        {
            if (ModelState.IsValid)
            {
                UserModel userDel = await UserManager.FindByEmailAsync(user.Email);
                //UserModel userDel = null;

                if (userDel != null)
                {
                    userDel.UserName = user.UserName;
                    userDel.Email = user.Email;
                    userDel.PhoneNumber = user.PhoneNumber;
                    IdentityResult result = await UserManager.UpdateAsync(userDel);

                    if (result.Succeeded)
                    {
                        TempData["Message"] = "Profile edited success";
                        return RedirectToAction("UserProfile");
                    }
                    else
                    {
                        TempData["Message"] = "Profile does't edit";
                        return RedirectToAction("UserProfile");
                    }
                }
            }
            return RedirectToAction("UserProfile");
        }

        public ActionResult Index()
        {
            return View();
        }


    }
}