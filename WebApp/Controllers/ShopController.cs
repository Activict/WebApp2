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
                            return View(model);
                        }
                        productDb.Name = model.Name;
                        productDb.Price = model.Price;
                        productDb.ImageName = model.ImageName;
                        productDb.Description = model.Description;

                        ShopCategoryDB category = await db.ShopCategories.FindAsync(model.ShopCategoryId);
                        if (category != null)
                        {
                            productDb.ShopCategoryId = category.Id;
                            productDb.ShopCategoryName = category.Name;
                        }
                        else
                        {
                            return View(model.Id);
                        }

                        db.Entry(productDb).State = System.Data.Entity.EntityState.Modified;
                        await db.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                }
                return View(model.Id);
            }
        }

        public async Task<ActionResult> ProductDetails(int id)
        {
            using (Db db = new Db())
            {
                ShopProductDB productDb = await db.ShopProducts.FindAsync(id);

                if (productDb != null)
                {
                    return View(new ShopProductVM(productDb));
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> ProductDelete(int id)
        {
            using (Db db = new Db())
            {
                ShopProductDB productDb = await db.ShopProducts.FindAsync(id);

                if (productDb != null)
                {
                    return View(new ShopProductVM(productDb));
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> ProductDelete(ShopProductVM model)
        {
            using (Db db = new Db())
            {
                ShopProductDB productDb = await db.ShopProducts.FindAsync(model.Id);

                if (productDb != null)
                {
                    db.ShopProducts.Remove(productDb);
                    db.SaveChanges();

                    if (!db.ShopProducts.Any(m => m.Id == model.Id))
                    {

                    }
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ProductAdd()
        {
            ShopProductVM product = new ShopProductVM();
            using (Db db = new Db())
            {
                product.ShopCategories = new SelectList(db.ShopCategories.ToList(), "Id", "Name");
            }
            return View(product);
        }

        [HttpPost]
        public async Task<ActionResult> ProductAdd(ShopProductVM model)
        {
            if (ModelState.IsValid)
            {
                using (Db db = new Db())
                {
                    if (db.ShopProducts.Any(m => m.Name == model.Name))
                    {
                        ModelState.AddModelError("", "This name product alredy exist");
                        return View(model);
                    }

                    ShopProductDB productAdd = new ShopProductDB
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Price = model.Price,
                        ShopCategoryId = model.ShopCategoryId,
                        ShopCategoryName = db.ShopCategories.Find(model.ShopCategoryId).Name
                    };

                    db.ShopProducts.Add(productAdd);
                    await db.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index");
        }
    }
}