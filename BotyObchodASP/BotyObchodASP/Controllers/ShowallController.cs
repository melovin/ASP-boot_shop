using BotyObchodASP.Models;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.IsisMtt.X509;

namespace BotyObchodASP.Controllers
{
    public class ShowallController : Controller
    {
        MyContext myContex = new();
        public IActionResult Index(int from = 0, int to = 12, List<string> colors = null, List<int> sizes = null, bool cancel = false, string orderby = "")
        {
            ViewBag.From = from;
            ViewBag.To = to;
            ViewBag.Categories = myContex.TbCategories.ToList();
            ViewBag.SelectedColors = colors;
            ViewBag.SelectedSizes = sizes;
            
            if (cancel)
            {
                colors.Clear();
                sizes.Clear();
            }
            List<TbProduct> products = null;
            if(colors.Count != 0)
            {
                products = myContex.TbProducts.Include(x => x.TbStocks).ThenInclude(x => x.IdColorNavigation).Include(x => x.TbPictures).ToList().Where(d => d.Active && d.TbStocks.Select(x => x.IdColorNavigation.Name).Intersect(colors).Any()).ToList();
            }
            else
            {
                products = myContex.TbProducts.Where(x => x.Active).Include(x => x.TbStocks).Include(x => x.TbPictures).ToList();
            }
            if (sizes.Count != 0)
            {
                products = products.Where(d => d.TbStocks.Select(x => x.Size).Intersect(sizes).Any()).ToList();
            }
            if (orderby == "pricelth")
            {
                products = products.OrderBy(p => p.TbStocks.Min(s => s.Price)).ToList();
            }
            else if (orderby == "pricehtl")
            {
                products = products.OrderBy(p => p.TbStocks.Max(s => s.Price)).ToList();
            }
            else if (orderby == "abcaz")
            {
                products = products.OrderBy(p => p.Name).ToList();
            }
            else if (orderby == "abcza")
            {
                products = products.OrderByDescending(p => p.Name).ToList();
            }
            ViewBag.Products = products.ToList();
            ViewBag.Colors = myContex.TbColors.ToList().DistinctBy(x => x.Name).OrderBy(x => x.Name);
            ViewBag.Sizes = myContex.TbStocks.ToList().DistinctBy(x => x.Size).OrderBy(x => x.Size);
            return View();
        }
    }
}
