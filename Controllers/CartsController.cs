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

namespace GeorgianBudgetSaver.Controllers
{
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("cart") != null)
            {
                List<Cart> cartList = JsonConvert.DeserializeObject<List<Cart>>(HttpContext.Session.GetString("cart"));
                if(cartList.Count() > 0)
                {
                    var applicationDbContext = await _context.Books.Include(b => b.CourseProgram).ToListAsync();
                    cartList.ForEach((obj) =>
                    {
                        Cart cart = new Cart
                        {
                            BookId = obj.BookId,
                            Quantity = obj.Quantity
                        };
                        applicationDbContext = applicationDbContext.Where(b => b.BookId == obj.BookId).ToList();
                    });
                    return View(applicationDbContext);

                }
            }
            return View(new List<Book>());
        }
        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            //remove from session
            List<Cart> cartList = JsonConvert.DeserializeObject<List<Cart>>(HttpContext.Session.GetString("cart"));
            cartList = cartList.Where(b => b.BookId != productId).ToList();
            string jsonString = System.Text.Json.JsonSerializer.Serialize(cartList);

            HttpContext.Session.SetString("cart", jsonString);

            return RedirectToAction("Index");

        }
        [Authorize]
        public IActionResult Checkout()
        {

            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Address,City,Province,PostalCode,Phone,Email,OrderDate,Total")] Order order)
        {
            
            /*if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            *//* ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", book.AccountId);*//*
            ViewData["CourseProgramId"] = new SelectList(_context.CoursePrograms, "CourseProgramId", "CourseProgramId", book.CourseProgramId);*/
            return View();
        }
    }
}
