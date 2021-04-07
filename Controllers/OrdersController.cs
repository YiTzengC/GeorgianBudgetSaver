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
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize]
        // GET: Orders
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Administrator"))
            {
                var allOrders = await _context.Orders.ToListAsync();
                return View(allOrders);
            }
            var orders = await _context.Orders.Where(order => order.CustomerId == User.Identity.Name).OrderBy(o => o.OrderDate).ToListAsync();
            return View(orders);
        }

        [Authorize(Roles = "Customer")]
        // GET: Orders/Create
        public IActionResult Create()
        {
            Models.Order order = JsonConvert.DeserializeObject<Models.Order>(HttpContext.Session.GetString("order"));
            _context.Add(order);
            _context.SaveChanges();
            string jsonString = System.Text.Json.JsonSerializer.Serialize(order);
            HttpContext.Session.SetString("order", jsonString);
            return RedirectToAction("Create", "OrderDetails");
        }

    }
}
