using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GeorgianBudgetSaver.Data;
using GeorgianBudgetSaver.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace GeorgianBudgetSaver.Controllers
{
    public class OrderDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        // GET: OrderDetails
        public IActionResult DetailsWithOrder(int orderId)
        {
            var applicationDbContext = _context.OrderDetails.Include(o => o.Book).Where(od => od.OrderId == orderId).OrderBy(o => o.Book.Title).ToList();
            applicationDbContext.ForEach(od =>
            {
                od.Book.CourseProgram = _context.CoursePrograms.Find(od.Book.CourseProgramId);
            });
            ViewBag.Order = _context.Orders.Find(orderId);
            return View(applicationDbContext);
        }

        [Authorize(Roles = "Customer")]
        public IActionResult Create()
        {
            Models.Order order = JsonConvert.DeserializeObject<Models.Order>(HttpContext.Session.GetString("order"));
            List<Cart> cartList = JsonConvert.DeserializeObject<List<Cart>>(HttpContext.Session.GetString("cart"));
            cartList.ForEach(cart =>
            {
                _context.Add(new OrderDetail
                {
                    OrderId = order.OrderId,
                    BookId = cart.BookId
                });
                _context.SaveChanges();

            });
            return RedirectToAction("UpdateStock", "Books");
        }
        private bool OrderDetailExists(int id)
        {
            return _context.OrderDetails.Any(e => e.OrderDetailId == id);
        }
    }
}
