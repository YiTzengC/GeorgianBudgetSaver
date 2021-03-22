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

namespace GeorgianBudgetSaver.Controllers
{
    [Authorize(Roles ="Administrator")]
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Index(int? id, string searchString)
        {
            var books = from m in _context.Books.Include(b => b.CourseProgram)
                        select m;
            if (id != null)
            {
                Console.WriteLine($"id: {id}");
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
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Books.Include(b => b.CourseProgram);
            return View(await applicationDbContext.ToListAsync());
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
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                /*.Include(b => b.Account)*/
                .Include(b => b.CourseProgram)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            /*ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "Username");*/
            ViewData["CourseProgramId"] = new SelectList(_context.CoursePrograms, "CourseProgramId", "Title");
            
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,Title,Author,BoughtDate,Price,InStock,CourseProgramId,AccountId")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           /* ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", book.AccountId);*/
            ViewData["CourseProgramId"] = new SelectList(_context.CoursePrograms, "CourseProgramId", "CourseProgramId", book.CourseProgramId);
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("BookId,Title,Author,BoughtDate,Price,InStock,CourseProgramId,AccountId")] Book book)
        {
            if (id != book.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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


    }
}
