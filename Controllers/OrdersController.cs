﻿using System;
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
        [Authorize(Roles = "Customer")]
        // GET: Orders
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Administrator")) {
                var allOrders = await _context.Orders.ToListAsync();
                return View(allOrders);
            }
            var orders = await _context.Orders.Where(order => order.CustomerId == User.Identity.Name).ToListAsync();
            return View(orders);
        }
        [Authorize]
        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            return RedirectToAction("DetailsWithOrder", "OrderDetails", order);
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

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,CustomerId,Address,City,Province,PostalCode,Phone,Email,OrderDate,Total")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }*/

        // GET: Orders/Edit/5
        /* public async Task<IActionResult> Edit(int? id)
         {
             if (id == null)
             {
                 return NotFound();
             }

             var order = await _context.Orders.FindAsync(id);
             if (order == null)
             {
                 return NotFound();
             }
             return View(order);
         }*/

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /* [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> Edit(int id, [Bind("OrderId,CustomerId,Address,City,Province,PostalCode,Phone,Email,OrderDate,Total")] Order order)
         {
             if (id != order.OrderId)
             {
                 return NotFound();
             }

             if (ModelState.IsValid)
             {
                 try
                 {
                     _context.Update(order);
                     await _context.SaveChangesAsync();
                 }
                 catch (DbUpdateConcurrencyException)
                 {
                     if (!OrderExists(order.OrderId))
                     {
                         return NotFound();
                     }
                     else
                     {
                         throw;
                     }
                 }
                 return RedirectToAction(nameof(Index));
             }
             return View(order);
         }*/

        // GET: Orders/Delete/5
        /*public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }*/

        // POST: Orders/Delete/5
        /*[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }*/
    }
}