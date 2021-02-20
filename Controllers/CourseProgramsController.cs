using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GeorgianBudgetSaver.Data;
using GeorgianBudgetSaver.Models;

namespace GeorgianBudgetSaver.Controllers
{
    public class CourseProgramsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseProgramsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CoursePrograms
        public async Task<IActionResult> Index()
        {
            return View(await _context.CoursePrograms.ToListAsync());
        }

        // GET: CoursePrograms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseProgram = await _context.CoursePrograms
                .FirstOrDefaultAsync(m => m.CourseProgramId == id);
            if (courseProgram == null)
            {
                return NotFound();
            }

            return View(courseProgram);
        }

        // GET: CoursePrograms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CoursePrograms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseProgramId,Title")] CourseProgram courseProgram)
        {
            if (ModelState.IsValid)
            {
                _context.Add(courseProgram);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(courseProgram);
        }

        // GET: CoursePrograms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseProgram = await _context.CoursePrograms.FindAsync(id);
            if (courseProgram == null)
            {
                return NotFound();
            }
            return View(courseProgram);
        }

        // POST: CoursePrograms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseProgramId,Title")] CourseProgram courseProgram)
        {
            if (id != courseProgram.CourseProgramId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(courseProgram);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseProgramExists(courseProgram.CourseProgramId))
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
            return View(courseProgram);
        }

        // GET: CoursePrograms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseProgram = await _context.CoursePrograms
                .FirstOrDefaultAsync(m => m.CourseProgramId == id);
            if (courseProgram == null)
            {
                return NotFound();
            }

            return View(courseProgram);
        }

        // POST: CoursePrograms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var courseProgram = await _context.CoursePrograms.FindAsync(id);
            _context.CoursePrograms.Remove(courseProgram);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseProgramExists(int id)
        {
            return _context.CoursePrograms.Any(e => e.CourseProgramId == id);
        }
    }
}
