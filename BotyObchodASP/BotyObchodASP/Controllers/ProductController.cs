using BotyObchodASP.Attributes;
using BotyObchodASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BotyObchodASP.Controllers
{
    [Categories]
    public class ProductController : Controller
    {
        private MyContext myContext = new();
        public IActionResult Index(int productId, int variantId)
        {
            ViewBag.ProductId = productId;
            ViewBag.VariantId = variantId;
            ViewBag.Product = myContext.TbProducts.Include(x => x.TbPictures).FirstOrDefault(x => x.Id == productId && x.Active);
            var variant = myContext.TbStocks.Include(x => x.IdColorNavigation).FirstOrDefault(x => x.Id == variantId && x.Active);
            ViewBag.Variant = variant;
            ViewBag.Variants = myContext.TbStocks.Where(x => x.IdProduct == productId && x.Active);
            var categories = myContext.TbCategoriesDetails.Where(x => x.IdProduct == productId).Include(x => x.IdCategoryNavigation).Select(x => x.IdCategoryNavigation);
            ViewBag.Category = categories;
            ViewBag.Products = myContext.TbProducts.Include(x => x.TbStocks).Include(x => x.TbPictures).Where(x => x.Active).Take(4);
            ViewBag.Colors = myContext.TbStocks.Where(x => x.IdProduct == productId && x.Active ).Include(x => x.IdColorNavigation).ToList().DistinctBy(x => x.IdColorNavigation.Name);
            ViewBag.Sizes = myContext.TbStocks.Where(x => x.IdProduct == productId && variant.IdColor == x.IdColor && x.Active).Select(x => x.Size);
            return View();
        }
        public IActionResult DetailPrep(int productId, int size, int colorId)
        {
            var variants = myContext.TbStocks.Where(x => x.IdProduct == productId && x.Active);
            TbStock variant = variants.FirstOrDefault(x => x.IdColor == colorId && x.Size == size);
            if(variant is null)
            {
                variant = variants.FirstOrDefault(x => x.IdColor == colorId);
            }

            return RedirectToAction("Index", new { productId = productId, variantId = variant.Id });
        }
    }
}
