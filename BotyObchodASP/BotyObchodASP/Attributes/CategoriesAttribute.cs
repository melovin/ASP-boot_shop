using BotyObchodASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BotyObchodASP.Attributes
{
    public class CategoriesAttribute : Attribute, IActionFilter
    {
        MyContext myContext = new();
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var ctrl = context.Controller as Controller;
            ctrl.ViewBag.Categories = myContext.TbCategories.ToList();
        }
    }
}
