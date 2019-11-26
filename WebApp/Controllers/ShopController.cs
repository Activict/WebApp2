using System;
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
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index()
        {
            ViewBag.Menu = "Shop";
            using (Db db = new Db())
            {
                List<ShopProductVM> products = db.ShopProducts.ToArray().Select(m => new ShopProductVM(m)).ToList();
                
                if (products != null)
                {
                    return View(products);
                }
            }
            
            return View();
        }
        public ActionResult Category(string id)
        {
            ViewBag.Menu = "Shop";
            using (Db db = new Db())
            {
                List<ShopProductVM> products = db.ShopProducts.ToArray()
                                                              .Where(m => m.ShopCategoryName == id)
                                                              .Select(m => new ShopProductVM(m))
                                                              .ToList();
                return View("Index", products);
            }
        }

        public ActionResult CategoryPartial()
        {
            List<ShopCategoryVM> categories;
            using (Db db = new Db())
            {
                categories = db.ShopCategories.ToArray().Select(m => new ShopCategoryVM(m)).ToList();
            }
            return PartialView("_CategoryPartial", categories);
        }

        [HttpGet]
        public async Task<ActionResult> ProductEdit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            using (Db db = new Db())
            {
                ShopProductVM product = new ShopProductVM(await db.ShopProducts.FindAsync((int)id));

                if (product != null)
                {
                    product.ShopCategories = new SelectList(db.ShopCategories.ToList(), "Id", "Name");
                    return View(product);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> ProductEdit(ShopProductVM model)
        {
            using (Db db = new Db())
            {
                if (ModelState.IsValid)
                {
                    ShopProductDB productDb = await db.ShopProducts.FindAsync(model.Id);

                    if (productDb != null)
                    {
                        if (db.ShopProducts.Where(m => m.Id != model.Id).Any(m => m.Name == model.Name))
                        {
                            ModelState.AddModelError("", "This name product already exist");
                        }
                        productDb.Name = model.Name;
                        productDb.Price = model.Price;
                        productDb.ImageName = model.ImageName;

                        if (model.ShopCategoryId != int.Parse(model.ShopCategoryName))
                        {
                            ShopCategoryDB category = await db.ShopCategories.FindAsync(int.Parse(model.ShopCategoryName));
                            if (category != null)
                            {
                                productDb.ShopCategoryId = category.Id;
                                productDb.ShopCategoryName = category.Name;
                            }
                        }
                        
                        db.Entry(productDb).State = System.Data.Entity.EntityState.Modified;
                        await db.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                }
                return View(model);   
            }
        }
    }
}