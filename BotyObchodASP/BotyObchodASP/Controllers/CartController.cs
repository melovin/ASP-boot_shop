using BotyObchodASP.Attributes;
using BotyObchodASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BotyObchodASP.Controllers
{
    [Categories]
    public class CartController : Controller
    {
        MyContext myContext = new();
        public IActionResult Index(int variantId = 0, int q = -1, bool deleteAll = false, bool delete = false)
        {
            if(deleteAll) //mazu vsechny
            {
                HttpContext.Session.Remove("order");
                ViewBag.Variants = null;
                return View();
            }

            TbStock v = myContext.TbStocks.Include(x => x.IdColorNavigation).FirstOrDefault(x => x.Id == variantId);
            //ViewBag.Variant = v;
            //ViewBag.Product = myContext.TbProducts.Include(x => x.TbPictures).Include(x => x.TbStocks).FirstOrDefault(x => x.TbStocks.Contains(v));
            ViewBag.Products = myContext.TbProducts.Include(x => x.TbPictures).ToList();
            if (delete) // mazu jeden produkt
            {
                CartPrep cart = JsonConvert.DeserializeObject<CartPrep>(HttpContext.Session.GetString("order"));
                cart.OrderDetail.Remove(cart.OrderDetail.FirstOrDefault(x => x.IdStock == variantId));
                if(cart.OrderDetail.Count == 0)
                {
                    HttpContext.Session.Remove("order");
                    ViewBag.Variants = null;
                    return View();
                }
                string order = JsonConvert.SerializeObject(cart);
                HttpContext.Session.SetString("order", order);
                ViewBag.Variants = myContext.TbStocks.Include(x => x.IdColorNavigation).ToList().Where(x => cart.OrderDetail.Select(x => x.IdStock).Contains(x.Id)).ToList();
                ViewBag.Quantity = cart.OrderDetail;
                return View();
            }

            if(HttpContext.Session.Get("order") == null) // uplne novy produkt
            {
                if (q < 0)
                    q = 1;
                double price = v.Price * q;
                CartPrep cart = new CartPrep();
                cart.Order = new() { CreationDate = DateTime.Now, State = "Nová objednávka" };
                cart.OrderDetail.Add(new() { IdStock = variantId, Price = price, Quantity = q, Tax = v.Tax, Discount = v.Discount });
                string order = JsonConvert.SerializeObject(cart);
                HttpContext.Session.SetString("order", order);
            }
            else if(q > 0) //zvedlo se množství přes input
            {
                CartPrep cart = JsonConvert.DeserializeObject<CartPrep>(HttpContext.Session.GetString("order"));
                cart.OrderDetail.FirstOrDefault(x => x.IdStock == variantId).Quantity = q;
                cart.OrderDetail.FirstOrDefault(x => x.IdStock == variantId).Price = v.Price * q;
                string order = JsonConvert.SerializeObject(cart);
                HttpContext.Session.SetString("order", order);
            }
            else if(variantId != 0) //item added to cart
            {
                CartPrep cart = JsonConvert.DeserializeObject<CartPrep>(HttpContext.Session.GetString("order"));
                
                if (cart.OrderDetail.Select(x => x.IdStock).Contains(variantId)) //zvedni mnozstvi již přidaného stejného
                {
                    
                    cart.OrderDetail.FirstOrDefault(x => x.IdStock == variantId).Quantity++;
                    cart.OrderDetail.FirstOrDefault(x => x.IdStock == variantId).Price = v.Price * cart.OrderDetail.FirstOrDefault(x => x.IdStock == variantId).Quantity;
                }
                else //uz neco v kosiku mam a pridavam novy produkt
                {
                    cart.OrderDetail.Add(new() { IdStock = variantId, Price = v.Price, Quantity = 1, Tax = v.Tax, Discount = v.Discount });
                }
                string order = JsonConvert.SerializeObject(cart);
                HttpContext.Session.SetString("order", order);
            }
            CartPrep cartFinal = JsonConvert.DeserializeObject<CartPrep>(HttpContext.Session.GetString("order"));
            ViewBag.Variants = myContext.TbStocks.Include(x => x.IdColorNavigation).ToList().Where(x => cartFinal.OrderDetail.Select(x => x.IdStock).Contains(x.Id)).ToList();
            ViewBag.Quantity = cartFinal.OrderDetail;
            return View();
        }
        public IActionResult Delivery(int delivery = -1)
        {
            ViewBag.Products = myContext.TbProducts.Include(x => x.TbPictures).ToList();
            CartPrep cartFinal = JsonConvert.DeserializeObject<CartPrep>(HttpContext.Session.GetString("order"));
            ViewBag.Variants = myContext.TbStocks.Include(x => x.IdColorNavigation).ToList().Where(x => cartFinal.OrderDetail.Select(x => x.IdStock).Contains(x.Id)).ToList();
            ViewBag.Quantity = cartFinal.OrderDetail;
            ViewBag.Deliveries = myContext.TbDeliveries.ToList().Where(x => x.Active);
            if(delivery >= 0)
            {
                cartFinal.Order.IdDelivery = delivery;
                string order = JsonConvert.SerializeObject(cartFinal);
                HttpContext.Session.SetString("order", order);
                return RedirectToAction("Payment");
            }
            return View();
        }
        public IActionResult Payment(int payment = -1)
        {
            ViewBag.Products = myContext.TbProducts.Include(x => x.TbPictures).ToList();
            CartPrep cartFinal = JsonConvert.DeserializeObject<CartPrep>(HttpContext.Session.GetString("order"));
            ViewBag.Variants = myContext.TbStocks.Include(x => x.IdColorNavigation).ToList().Where(x => cartFinal.OrderDetail.Select(x => x.IdStock).Contains(x.Id)).ToList();
            ViewBag.Quantity = cartFinal.OrderDetail;
            ViewBag.Delivery = myContext.TbDeliveries.FirstOrDefault(x => x.Id == cartFinal.Order.IdDelivery);
            ViewBag.Payments = myContext.TbPayments.ToList().Where(x => x.Active);
            if (payment >= 0)
            {
                cartFinal.Order.IdPayment = payment;
                string order = JsonConvert.SerializeObject(cartFinal);
                HttpContext.Session.SetString("order", order);
                return RedirectToAction("Customer", new TbCustomer());
            }
            return View();
        }
        public IActionResult Customer(TbCustomer customer)
        {
            ViewBag.Products = myContext.TbProducts.Include(x => x.TbPictures).ToList();
            CartPrep cartFinal = JsonConvert.DeserializeObject<CartPrep>(HttpContext.Session.GetString("order"));
            ViewBag.Variants = myContext.TbStocks.Include(x => x.IdColorNavigation).ToList().Where(x => cartFinal.OrderDetail.Select(x => x.IdStock).Contains(x.Id)).ToList();
            ViewBag.Quantity = cartFinal.OrderDetail;
            ViewBag.Delivery = myContext.TbDeliveries.FirstOrDefault(x => x.Id == cartFinal.Order.IdDelivery);
            ViewBag.Payment = myContext.TbPayments.FirstOrDefault(x => x.Id == cartFinal.Order.IdPayment);
            if(customer.Name != null)
            {
                cartFinal.Customer = customer;
                string order = JsonConvert.SerializeObject(cartFinal);
                HttpContext.Session.SetString("order", order);
                return RedirectToAction("Summary");
            }           
            return View(customer);
        }
        public IActionResult Summary()
        {
            ViewBag.Products = myContext.TbProducts.Include(x => x.TbPictures).ToList();
            CartPrep cartFinal = JsonConvert.DeserializeObject<CartPrep>(HttpContext.Session.GetString("order"));
            ViewBag.Variants = myContext.TbStocks.Include(x => x.IdColorNavigation).ToList().Where(x => cartFinal.OrderDetail.Select(x => x.IdStock).Contains(x.Id)).ToList();
            ViewBag.Quantity = cartFinal.OrderDetail;
            ViewBag.Delivery = myContext.TbDeliveries.FirstOrDefault(x => x.Id == cartFinal.Order.IdDelivery);
            ViewBag.Payment = myContext.TbPayments.FirstOrDefault(x => x.Id == cartFinal.Order.IdPayment);
            ViewBag.Customer = cartFinal.Customer;

            return View();
        }
        public IActionResult Confirmation()
        {
            CartPrep cartFinal = JsonConvert.DeserializeObject<CartPrep>(HttpContext.Session.GetString("order"));
            myContext.TbCustomers.Add(cartFinal.Customer);
            myContext.SaveChanges();
            cartFinal.Order.IdCustomer = myContext.TbCustomers.ToList()[myContext.TbCustomers.ToList().Count - 1].Id;
            myContext.TbOrders.Add(cartFinal.Order);
            myContext.SaveChanges();
            int orderId = myContext.TbOrders.ToList()[myContext.TbOrders.ToList().Count - 1].Id;
            foreach (var item in cartFinal.OrderDetail)
            {
                item.IdOrder = orderId;
            }
            myContext.AddRange(cartFinal.OrderDetail);
            myContext.SaveChanges();
            HttpContext.Session.Remove("order");
            return View();
        }
    }
}
