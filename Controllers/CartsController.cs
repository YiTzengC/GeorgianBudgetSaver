using GeorgianBudgetSaver.Data;
using GeorgianBudgetSaver.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stripe;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Stripe.Checkout;

namespace GeorgianBudgetSaver.Controllers
{
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        IConfiguration _configuration;

        public CartsController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        /*public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("cart") != null)
            {
                List<Cart> cartList = JsonConvert.DeserializeObject<List<Cart>>(HttpContext.Session.GetString("cart"));
                List<Book> books = new List<Book>();
                if (cartList.Count() > 0)
                {
                    var applicationDbContext = await _context.Books.Include(b => b.CourseProgram).ToListAsync();
                    cartList.ForEach((obj) =>
                    {
                        books.Add(applicationDbContext.Find(b => b.BookId == obj.BookId));
                    });
                    return View(books);

                }
            }
            return View(new List<Book>());
        }*/

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            //remove from session
            List<Cart> cartList = JsonConvert.DeserializeObject<List<Cart>>(HttpContext.Session.GetString("cart"));
            cartList = cartList.Where(b => b.BookId != productId).ToList();
            string jsonString = System.Text.Json.JsonSerializer.Serialize(cartList);

            HttpContext.Session.SetString("cart", jsonString);

            return RedirectToAction("Checkout");

        }
        public async Task<IActionResult> Checkout()
        {
            int items = 0;
            decimal priceSum = 0;
            List<Book> books = new List<Book>();
            List<string> provinces = new List<string>();
            provinces.Add("Alberta");
            provinces.Add("British Columbia");
            provinces.Add("Manitoba");
            provinces.Add("New Brunswick");
            provinces.Add("Newfoundland and Labrador");
            provinces.Add("Northwest Territories");
            provinces.Add("Nova Scotia");
            provinces.Add("Nunavut");
            provinces.Add("Ontario");
            provinces.Add("Prince Edward Island");
            provinces.Add("Quebec");
            provinces.Add("Saskatchewan");
            provinces.Add("Yukon");
            if (HttpContext.Session.GetString("cart") != null)
            {
                List<Cart> cartList = JsonConvert.DeserializeObject<List<Cart>>(HttpContext.Session.GetString("cart"));
                if (cartList.Count() > 0)
                {
                    var applicationDbContext = await _context.Books.Include(b => b.CourseProgram).ToListAsync();
                    cartList.ForEach((obj) =>
                    {
                        items = items + obj.Quantity;
                        priceSum = priceSum + obj.Quantity * obj.Price;
                        books.Add(applicationDbContext.Find(b => b.BookId == obj.BookId));
                    });
                    ViewBag.Books = books;
                }
            }
            ViewBag.Provinces = provinces;
            ViewBag.Price = priceSum;
            ViewBag.Quantity = items;
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Customer")]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout([Bind("FirstName,LastName,Address,City,Province,PostalCode,Phone,Email,OrderDate,Total")] Models.Order order)
        {
            /*order.Total = 0;*/
            order.OrderDate = DateTime.Now;
            order.CustomerId = User.Identity.Name;
            List<Cart> cartList = JsonConvert.DeserializeObject<List<Cart>>(HttpContext.Session.GetString("cart"));
            cartList.ForEach((n) =>
            {
                Console.WriteLine($"id: {n.BookId}, q: {n.Quantity}, p: {n.Price}");
            });
            order.Total = (from cart in cartList select cart.Price * cart.Quantity).Sum();
            Console.WriteLine($"order.Total: {order.Total}");
            string jsonString = System.Text.Json.JsonSerializer.Serialize(order);
            HttpContext.Session.SetString("order", jsonString);

            return RedirectToAction("Payment");
        }
        [Authorize(Roles = "Customer")]
        public IActionResult Payment()
        {
            var orderObj = JsonConvert.DeserializeObject<Models.Order>(HttpContext.Session.GetString("order"));
            Console.WriteLine($"obj: {orderObj.Total}");
            ViewData["Total"] = orderObj.Total;
            ViewData["PublishableKey"] = _configuration.GetSection("Stripe")["PublishableKey"];

            return View();
        }
        [Authorize(Roles = "Customer")]
        [HttpPost]
        public IActionResult ProcessPayment()
        {
            Models.Order orderObj = JsonConvert.DeserializeObject<Models.Order>(HttpContext.Session.GetString("order"));
            //secret key
            StripeConfiguration.ApiKey = _configuration.GetSection("Stripe")["SecretKey"];

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                  "card",
                },
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                      UnitAmount = (long?)(orderObj.Total * 100),
                      Currency = "cad",
                      ProductData = new SessionLineItemPriceDataProductDataOptions
                      {
                        Name = "Georgian Budget Saver Purchase",
                      },
                    },
                    Quantity = 1,
                  },
                },
                Mode = "payment",
                SuccessUrl = "https://" + Request.Host + "/Orders/Create",
                CancelUrl = "https://" + Request.Host + "/Carts/Checkout",
            };
            var service = new SessionService();
            Session session = service.Create(options);
            return Json(new { id = session.Id });
        }
    }
}
