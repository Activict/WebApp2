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
                        // sdd to Session Cart
                        bool checkFirst = true;

                        var listCart = (List<ShopCartVM>)Session["Cart"];

                        if (Session["Cart"] != null)
                        {
                            for (int i = 0; i < listCart.Count; i++)
                            {
                                if (listCart[i].ProductId == productDb.Id)
                                {
                                    listCart[i].AmountProduct++;
                                    Session["Cart"] = listCart;
                                    checkFirst = false;
                                    break;
                                }
                            }
                        }
                        // add to ShopCartDB
                        string userId = (string)Session["UserId"];

                        if (checkFirst)
                        {
                            ShopCartDB addProduct = new ShopCartDB
                            {
                                UserId = (string)Session["UserId"],
                                ProductId = productDb.Id,
                                ProductName = productDb.Name,
                                Description = productDb.Description,
                                Price = productDb.Price,
                                AmountProduct = 1,
                                Image = productDb.ImageName
                            };

                            db.ShopCarts.Add(addProduct);
                            db.SaveChanges();

                            addProduct = db.ShopCarts.Where(m => m.ProductId == addProduct.ProductId && m.UserId == userId).FirstOrDefault();
                            if (addProduct != null)
                            {
                                listCart.Add(new ShopCartVM(addProduct));
                                Session["Cart"] = listCart;
                            }
                        }
                        else
                        {
                            ShopCartDB addProduct = db.ShopCarts.Where(m => m.ProductName == productDb.Name &&
                                                                            m.UserId == userId).FirstOrDefault();
                            addProduct.AmountProduct++;
                            db.Entry(addProduct).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        //ViewBag. = "Product don't add to cart";
                        return RedirectToAction("Index", "Shop");
                    }
                }
            }

            if (((List<ShopCartVM>)Session["Cart"]).Count == 0)
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

            return PartialView("_CartPartial", model);
        }

        [HttpGet]
        public async Task<ActionResult> DeleteFromCart(int? id)
        {
            ViewBag.Menu = "Shop";

            if (id == null)
            {

            }
            else
            {
                using (Db db = new Db())
                {
                    ShopCartDB productCartDb = await db.ShopCarts.FindAsync(id);

                    if (productCartDb != null)
                    {
                        return View(new ShopCartVM(productCartDb));
                    }
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteFromCart(ShopCartVM model)
        {

            using (Db db = new Db())
            {
                ShopCartDB modelDb = await db.ShopCarts.FindAsync(model.Id);

                db.ShopCarts.Remove(modelDb);
                db.SaveChanges();

                ViewBag.MessageInfo = model.ProductName + " is delete";

                modelDb = await db.ShopCarts.FindAsync(model.Id);

                if (modelDb != null)
                {
                    ViewBag.MessageInfo = modelDb.ProductName + " is't delete";
                }
                else
                {
                    var listCart = (List<ShopCartVM>)Session["Cart"];

                    if (Session["Cart"] != null)
                    {
                        for (int i = 0; i < listCart.Count; i++)
                        {
                            if (listCart[i].Id == model.Id)
                            {
                                listCart.RemoveAt(i);
                                Session["Cart"] = listCart;
                                break;
                            }
                        }
                    }
                    //await Index(null);
                }
            }
            return RedirectToAction("Index");
        }
    }
}