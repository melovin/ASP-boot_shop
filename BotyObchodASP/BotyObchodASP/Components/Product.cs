using BotyObchodASP.Models;
using Microsoft.AspNetCore.Mvc;

namespace BotyObchodASP.Components
{
    public class Product : ViewComponent
    {
        public IViewComponentResult Invoke(TbProduct product)
        {
            this.ViewBag.Product = product;
            return View();
        }
    }
}
