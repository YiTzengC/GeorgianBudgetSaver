using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeorgianBudgetSaver.Data;
using GeorgianBudgetSaver.Models;

namespace GeorgianBudgetSaver.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseProgramsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CourseProgramsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CoursePrograms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseProgram>>> GetCoursePrograms()
        {
            return await _context.CoursePrograms.ToListAsync();
        }

        // GET: api/CoursePrograms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseProgram>> GetCourseProgram(int id)
        {
            var courseProgram = await _context.CoursePrograms.FindAsync(id);

            if (courseProgram == null)
            {
                return NotFound();
            }

            return courseProgram;
        }

        // PUT: api/CoursePrograms/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourseProgram(int id, CourseProgram courseProgram)
        {
            if (id != courseProgram.CourseProgramId)
            {
                return BadRequest();
            }

            _context.Entry(courseProgram).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseProgramExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CoursePrograms
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CourseProgram>> PostCourseProgram(CourseProgram courseProgram)
        {
            _context.CoursePrograms.Add(courseProgram);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourseProgram", new { id = courseProgram.CourseProgramId }, courseProgram);
        }

        // DELETE: api/CoursePrograms/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CourseProgram>> DeleteCourseProgram(int id)
        {
            var courseProgram = await _context.CoursePrograms.FindAsync(id);
            if (courseProgram == null)
            {
                return NotFound();
            }

            _context.CoursePrograms.Remove(courseProgram);
            await _context.SaveChangesAsync();

            return courseProgram;
        }

        private bool CourseProgramExists(int id)
        {
            return _context.CoursePrograms.Any(e => e.CourseProgramId == id);
        }
    }
}
