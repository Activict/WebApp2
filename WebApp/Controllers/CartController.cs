using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.Models.Data;
using WebApp.Models.Shop;

namespace WebApp.Controllers
{
    public class CartController : Controller
    {
        AppUserManager UserManager { get { return HttpContext.GetOwinContext().GetUserManager<AppUserManager>(); } }
        //IAuthenticationManager AuthenticationManager { get { return HttpContext.GetOwinContext().Authentication; } }
        // GET: Cart
        public async Task<ActionResult> Index(int? id)
        {
            ViewBag.Menu = "Shop";

            if (id != null)
            {
                using (Db db = new Db())
                {
                    ShopProductDB productDb = await db.ShopProducts.FindAsync(id);
                    if (productDb != null)
                    {
                        // add to ShopCartDB and Session Cart
                        int amountProduct = 0;
                        bool check = false;

                        var listCart = (List<ShopCartVM>)Session["Cart"];

                        if (Session["Cart"] != null)
                        {
                            for (int i = 0; i < listCart.Count-1; i++)
                            {
                                if (listCart[i].Id == productDb.Id)
                                {
                                    amountProduct = listCart[i].AmountProduct++;
                                    Session["Cart"] = listCart;
                                    check = true;
                                    break;
                                }
                            }
                        }
                        ShopCartDB addProduct = db.ShopCarts.Where(m => m.ProductName == productDb.Name && 
                                                                        m.UserId == (string)Session["UserId"]).FirstOrDefault();
                        addProduct.AmountProduct++;

                        if (!check)
                        {
                            listCart.Add(new ShopCartVM(addProduct));
                            Session["Cart"] = listCart;
                        }

                        //var temp = Session["Cart"];
                        if (addProduct.AmountProduct == 1)
                        {
                            db.ShopCarts.Add(addProduct);
                        }
                        else
                        {
                            db.Entry(addProduct).State = System.Data.Entity.EntityState.Modified;
                        }
                        
                        db.SaveChanges();
                    }
                    else
                    {
                        //ViewBag. = "Product don't add to cart";
                        return RedirectToAction("Index", "Shop");
                    }
                }
            }

            //var temp1 = Session["Cart"];

            if (((List<ShopCartVM>)Session["Cart"]).Count == 0 )
            {
                ViewBag.Message = "Cart is empty";
            }

            var cart = Session["Cart"] as List<ShopCartVM> ?? new List<ShopCartVM>();

            decimal total = 0m;

            foreach (var item in cart)
            {
                total += item.Total;
            }

            ViewBag.TotalPriceCart = total;
            
            return View(cart);
        }

        public ActionResult CartPartial()
        {
            if (Session["UserId"] == null)
            {
                //var user = HttpContext.User.Identity.Name.ToString();
                UserModel user = UserManager.FindByEmail(HttpContext.User.Identity.Name);
                
                if (user == null)
                {
                    return RedirectToAction("Logout", "Account");
                }

                Session["UserId"] = user.Id;

                using (Db db = new Db())
                {
                    Session["Cart"] = db.ShopCarts.ToArray().Where(m => m.UserId == user.Id).Select(m => new ShopCartVM(m)).ToList();
                }
            }

            ShopCartVM model = new ShopCartVM();

            if (Session["Cart"] != null)
            {
                foreach (var product in (List<ShopCartVM>)Session["Cart"])
                {
                    model.AmountProduct += product.AmountProduct;
                    model.Price += product.Total;
                }
            }
            else
            {
                model.AmountProduct = 0;
                model.Price = 0m;
            }

            return PartialView( "_CartPartial", model);
        }
    }
}