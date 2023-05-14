using BotyObchodASP.Attributes;
using BotyObchodASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BotyObchodASP.Controllers
{
    [Secured]
    public class AdminController : Controller
    {
        private MyContext myContext = new();
        public IActionResult Index()
        {
            ViewBag.Orders = myContext.TbOrders.Include(x => x.IdCustomerNavigation).Include(x => x.IdDeliveryNavigation).Include(x => x.IdPaymentNavigation).Include(x => x.TbOrderDetails).OrderByDescending(x =>x.CreationDate);
            ViewBag.Details = myContext.TbOrderDetails.Include(x => x.IdOrderNavigation).Include(x => x.IdStockNavigation).ToList();
            ViewBag.Stock = myContext.TbStocks.Include(x => x.IdProductNavigation).Include(x => x.IdColorNavigation).ToList();
            ViewBag.Products = myContext.TbProducts;
            return View();
        }
        public IActionResult Produkty()
        {
            ViewBag.Products = myContext.TbProducts.Include(x => x.TbStocks).Include(x => x.TbCategoriesDetails).Include(x => x.TbPictures).ToList();
            ViewBag.Categories = myContext.TbCategoriesDetails.Include(x => x.IdCategoryNavigation).ToList();
            return View();
        }
        public IActionResult Produktedit(int produktid)
        {
            ViewBag.Product = myContext.TbProducts.Include(x => x.TbCategoriesDetails).Include(x => x.TbStocks).Include(x => x.TbPictures).ToList().FirstOrDefault(x => x.Id == produktid);
            ViewBag.Cats = myContext.TbCategoriesDetails.Include(x => x.IdCategoryNavigation).Where(x => x.IdProduct == produktid).ToList();
            Editmodel newModel = new();
            newModel.Product = myContext.TbProducts.Include(x => x.TbCategoriesDetails).Include(x => x.TbPictures).ToList().FirstOrDefault(x => x.Id == produktid);
            newModel.Stocks = myContext.TbStocks.Where(x => x.IdProduct == produktid).Include(x => x.TbPictures).Include(x => x.IdColorNavigation).ToList();
            newModel.Categories = myContext.TbCategoriesDetails.Include(x => x.IdCategoryNavigation).Where(x => x.IdProduct == produktid).ToList();
            return View(newModel);
        }
        public IActionResult Changes(Editmodel newprodukt)
        {
            TbProduct product = myContext.TbProducts.Include(x => x.TbCategoriesDetails).Include(x => x.TbStocks).Include(x => x.TbPictures).ToList().FirstOrDefault(x => x.Id == newprodukt.Product.Id);
            product.Id = newprodukt.Product.Id;
            product.Name = newprodukt.Product.Name;
            product.Description = newprodukt.Product.Description;
            product.DetailedDescription = newprodukt.Product.DetailedDescription;
            product.Material = newprodukt.Product.Material;
            product.Type = newprodukt.Product.Type;
            product.Active = newprodukt.Product.Active;
            product.TbStocks = newprodukt.Stocks;
            //product.TbPictures = (product.TbPictures as List<TbPicture>).AddRange(newprodukt.Product.TbPictures);
            myContext.SaveChanges();
            return RedirectToAction("Produkty");
        }
        public IActionResult Variantadd(int produktid)
        {
            //myContext.TbProducts.FirstOrDefault(x => x.Id == produktid).TbStocks.Add(new TbStock());
            myContext.TbStocks.Add(new TbStock() { IdProduct = produktid, IdColor = 1, Active = true, Code ="XXX", Discount = 0, Price = 1000, Size = 20, Tax = 21, Quantity = 5  }) ;
            myContext.SaveChanges();
            return RedirectToAction("Produktedit",new {produktid = produktid});
        }
        public IActionResult Variantremove(int produktid, int variantid)
        {
            myContext.TbStocks.Remove(myContext.TbStocks.FirstOrDefault(x => x.Id == variantid));
            myContext.SaveChanges();
            return RedirectToAction("Produktedit", new { produktid = produktid });
        }
        public IActionResult Productremove(int produktid)
        {
            myContext.TbStocks.RemoveRange(myContext.TbStocks.Where(x => x.IdProduct == produktid));
            myContext.TbProducts.Remove(myContext.TbProducts.FirstOrDefault(x => x.Id == produktid));
            myContext.SaveChanges();
            return RedirectToAction("Produkty");
        }
        public IActionResult Productadd()
        {
            myContext.TbProducts.Add(new TbProduct() { Name = "newProduct",Description = "AddDescription", DetailedDescription = "AddDetailedDescription", Material = "AddMaterial", Type = "addType", Active = false});
            myContext.SaveChanges();
            myContext.TbStocks.Add(new TbStock() { IdProduct = myContext.TbProducts.OrderByDescending(x => x.Id).FirstOrDefault().Id, IdColor = 1, Size = 0, Quantity = 0, Price = 0, Discount = 0, Code = "XXX", Tax = 0, Active = false });
            myContext.SaveChanges();
            myContext.TbPictures.Add(new TbPicture() { IdProduct = myContext.TbProducts.OrderByDescending(x => x.Id).FirstOrDefault().Id, PrimaryImg = true, Path = "../img2/bota1.png", IdStock = myContext.TbStocks.OrderByDescending(x => x.Id).FirstOrDefault().Id });
            myContext.SaveChanges();
            return RedirectToAction("Produkty");
        }
        public IActionResult Deliverypayment()
        {
            ViewBag.Deliveries = myContext.TbDeliveries;
            ViewBag.Payments = myContext.TbPayments;
            return View();
        }
        public IActionResult Deliveryremove(int deliveryid)
        {
            foreach (TbOrder item in myContext.TbOrders)
            {
                if(item.IdDelivery == deliveryid)
                {
                    Response.WriteAsync(@"<script language='javascript'>alert('Zpusob dopravy nelze odstranit, je vyuzivan.')</script>");
                    return RedirectToAction("Deliverypayment");
                }               
            }
            myContext.TbDeliveries.Remove(myContext.TbDeliveries.FirstOrDefault(x => x.Id == deliveryid));
            myContext.SaveChanges();
            return RedirectToAction("Deliverypayment");
        }
        public IActionResult Paymentremove(int paymentid)
        {
            foreach (TbOrder item in myContext.TbOrders)
            {
                if (item.IdPayment == paymentid)
                {
                    Response.WriteAsync(@"<script language='javascript'>alert('Zpusob platby nelze odstranit, je vyuzivan.')</script>");
                    return RedirectToAction("Deliverypayment");
                }
            }
            myContext.TbPayments.Remove(myContext.TbPayments.FirstOrDefault(x => x.Id == paymentid));
            myContext.SaveChanges();
            return RedirectToAction("Deliverypayment");
        }
        public IActionResult Deliveryadd()
        {
            myContext.TbDeliveries.Add(new TbDelivery() { Name = "AddName", Price = 0, Active = false });
            myContext.SaveChanges();
            return RedirectToAction("Deliverypayment");
        }
        public IActionResult Paymentadd()
        {
            myContext.TbPayments.Add(new TbPayment() { Name = "AddName", Price = 0, Active = false });
            myContext.SaveChanges();
            return RedirectToAction("Deliverypayment");
        }
        public IActionResult Deliverypaymentedit(int delid) //vyrvarim si sablonu pro edit 
        {
            TbDelivery del = myContext.TbDeliveries.FirstOrDefault(x => x.Id == delid);
            Editdelivery delEdit = new();
            delEdit.Delivery = del;
            myContext.SaveChanges();
            return View(delEdit);
        }
        public IActionResult Deliverypaymentchanges(Editdelivery del)
        {
            TbDelivery oldDel = myContext.TbDeliveries.FirstOrDefault(x => x.Id == del.Delivery.Id);
            oldDel.Name = del.Delivery.Name;
            oldDel.Price = del.Delivery.Price;
            oldDel.Active = del.Delivery.Active;
            myContext.SaveChanges();
            return RedirectToAction("Deliverypayment");
        }
        public IActionResult Paymentedit(int payid) //vyrvarim si sablonu pro edit 
        {
            TbPayment pay = myContext.TbPayments.FirstOrDefault(x => x.Id == payid);
            Editpayment payEdit = new();
            payEdit.Payment = pay;
            myContext.SaveChanges();
            return View(payEdit);
        }
        public IActionResult Paymentchanges(Editpayment pay)
        {
            TbPayment oldPay = myContext.TbPayments.FirstOrDefault(x => x.Id == pay.Payment.Id);
            oldPay.Name = pay.Payment.Name;
            oldPay.Price = pay.Payment.Price;
            oldPay.Active = pay.Payment.Active;
            myContext.SaveChanges();
            return RedirectToAction("Deliverypayment");
        }

    }
}
