using BotyObchodASP.Attributes;
using BotyObchodASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BotyObchodASP.Controllers
{
    [Categories]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MyContext myContext = new();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //for (int i = 33; i <= 33; i++)
            //{
            //    this.myContext.TbStocks.Add(new TbStock()
            //    {
            //        IdProduct = i,
            //        IdColor = 1,
            //        Size = 24,
            //        Quantity = 10,
            //        Price = 1239.5,
            //        Discount = 10,
            //        Code = "DEV4ESD5",
            //        Tax = 21
            //    });
            //    this.myContext.TbPictures.Add(new TbPicture()
            //    {
            //        IdStock = null,
            //        Path = "../img2/bota1.png",
            //        PrimaryImg = false,
            //        IdProduct = i,
            //    });
            //}

            //this.myContext.SaveChanges();
            ViewBag.Products = myContext.TbProducts.Include(x => x.TbStocks).Include(x => x.TbPictures).Where(x => x.Active).Take(8);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}