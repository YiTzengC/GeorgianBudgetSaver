using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GeorgianBudgetSaver.Data;
using GeorgianBudgetSaver.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;

namespace GeorgianBudgetSaver.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Index(int? id, string searchString)
        {
            var books = from m in _context.Books.Include(b => b.CourseProgram).OrderBy(b => b.Title)
                        select m;
            if (!User.IsInRole("Administrator"))
            {
                books = books.Where(b => b.InStock == true);
            }
            if (id != null)
            {
                books = books.Where(m => m.CourseProgramId == id);

                return View(await books.ToListAsync());
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                Console.WriteLine($"searchString: {searchString}");
                books = books.Where(s => s.Title.Contains(searchString));
            }

            return View(await books.ToListAsync());
        }
        // GET: Books
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = await _context.Books.Include(b => b.CourseProgram).OrderBy(b => b.Title).ToListAsync();
            // do not dislpay item in cart
            if (HttpContext.Session.GetString("cart") != null)
            {
                List<Cart> cartList = JsonConvert.DeserializeObject<List<Cart>>(HttpContext.Session.GetString("cart"));
                cartList.ForEach((obj) =>
                {
                    applicationDbContext = applicationDbContext.Where(b => b.BookId != obj.BookId).ToList();
                });
            }
            if (!User.IsInRole("Administrator"))
            {
                applicationDbContext = applicationDbContext.Where(b => b.InStock == true).ToList();
            }
            return View(applicationDbContext);
        }

        /*[AllowAnonymous]
        [HttpPost]
        [ActionName("ListByProgram")]
        public async Task<IActionResult> Index(int id)
        {
            *//*if (id == null)
            {
                return NotFound();
            }*//*

            var books = await _context.Books
                 .Include(b => b.CourseProgram)
                 .Where(m => m.CourseProgramId == id).ToListAsync();

            return View(books);
        }*/

        // GET: Books/Details/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(int? bookId)
        {
            ViewBag.InCart = false;
            if (bookId == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.CourseProgram)
                .FirstOrDefaultAsync(m => m.BookId == bookId);
            if (book == null)
            {
                return NotFound();
            }
            string cartSession = HttpContext.Session.GetString("cart");
            if (cartSession != null) {
                List<Cart> cartList = JsonConvert.DeserializeObject<List<Cart>>(cartSession);
                ViewBag.InCart = cartList.Exists(b=>b.BookId == bookId);
            }

            return View(book);
        }
        [Authorize(Roles = "Administrator")]
        // GET: Books/Create
        public IActionResult Create()
        {
            /*ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "Username");*/
            ViewData["CourseProgramId"] = new SelectList(_context.CoursePrograms.OrderBy(p => p.Title), "CourseProgramId", "Title");

            return View();
        }
        [Authorize(Roles = "Administrator")]
        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,Title,Author,BoughtDate,Price,InStock,CourseProgramId")] Book book, IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                //check for photo
                if (photo.Length > 0)
                {
                    var fileName = Guid.NewGuid() + "-" + photo.FileName;
                    var des = Directory.GetCurrentDirectory() + "\\wwwroot\\img\\books\\" + fileName;
                    var stream = new FileStream(des, FileMode.Create);
                    await photo.CopyToAsync(stream);
                    book.Photo = fileName;
                }

                book.InStock = true;
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            /* ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", book.AccountId);*/
            ViewData["CourseProgramId"] = new SelectList(_context.CoursePrograms, "CourseProgramId", "CourseProgramId", book.CourseProgramId);
            return View(book);
        }
        [Authorize(Roles = "Administrator")]
        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            //only in stock book can be edited
            if (book.InStock == false)
            {

                return RedirectToAction("Index");
            }
            if (book == null)
            {
                return NotFound();
            }
            /*ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "Username", book.AccountId);*/
            ViewData["CourseProgramId"] = new SelectList(_context.CoursePrograms, "CourseProgramId", "Title", book.CourseProgramId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("Photo,BookId,Title,Author,BoughtDate,Price,InStock,CourseProgramId,AccountId")] Book book, IFormFile photo)
        {
            Console.WriteLine($"photo: {photo}");
            if (!book.InStock)
            {
                RedirectToAction("Index");
            }

            if (id != book.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (photo != null)
                    {
                        if (photo.Length > 0)
                        {
                            var fileName = Guid.NewGuid() + "-" + photo.FileName;
                            var des = Directory.GetCurrentDirectory() + "\\wwwroot\\img\\books\\" + fileName;
                            var stream = new FileStream(des, FileMode.Create);
                            await photo.CopyToAsync(stream);
                            book.Photo = fileName;
                        }
                    }
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookId))
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
            /*ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", book.AccountId);*/
            ViewData["CourseProgramId"] = new SelectList(_context.CoursePrograms, "CourseProgramId", "CourseProgramId", book.CourseProgramId);
            return View(book);
        }

        // GET: Books/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            /*var book = await _context.Books
                *//*.Include(b => b.Account)*//*
                .Include(b => b.CourseProgram)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);*/

            var book = await _context.Books.FindAsync(id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Books/Delete/5
        /*[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity, decimal price)
        {
            Console.WriteLine($"product id: {productId}, quantity: {quantity} ,price: {price}");
            string jsonString = "";
            if (HttpContext.Session.GetString("cart") == null)
            {
                List<Cart> cartlist = new List<Cart>();
                cartlist.Add(
                    new Cart
                    {
                        BookId = productId,
                        Quantity = quantity,
                        Price = price
                    }
                    );
                jsonString = System.Text.Json.JsonSerializer.Serialize(cartlist);

            }
            else
            {
                List<Cart> cartList = JsonConvert.DeserializeObject<List<Cart>>(HttpContext.Session.GetString("cart"));
                cartList.Add(
                    new Cart
                    {
                        BookId = productId,
                        Quantity = quantity,
                        Price = price
                    });
                jsonString = System.Text.Json.JsonSerializer.Serialize(cartList);
            }


            HttpContext.Session.SetString("cart", jsonString);

            return RedirectToAction("Checkout", "Carts");
        }
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateStock()
        {
            List<Cart> cartList = JsonConvert.DeserializeObject<List<Cart>>(HttpContext.Session.GetString("cart"));
            var applicationDbContext = await _context.Books.Include(b => b.CourseProgram).ToListAsync();
            cartList.ForEach((obj) =>
            {
                var book = applicationDbContext.Find(b => b.BookId == obj.BookId);
                book.InStock = false;
                _context.Update(book);
                _context.SaveChanges();
            });
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Orders");
        }


    }
}
