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
using System.Net;
using System.IO;
using System.Text;
using System.Net.Http;

namespace GeorgianBudgetSaver.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CourseProgramsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseProgramsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
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

            /*var courseProgram = await _context.CoursePrograms
                .FirstOrDefaultAsync(m => m.CourseProgramId == id);*/
            var courseProgram = await _context.CoursePrograms.FindAsync(id);

            if (courseProgram == null)
            {
                return NotFound();
            }

            _context.CoursePrograms.Remove(courseProgram);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            /*return View(courseProgram);*/
        }

        // POST: CoursePrograms/Delete/5
        /*[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var courseProgram = await _context.CoursePrograms.FindAsync(id);
            _context.CoursePrograms.Remove(courseProgram);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/

        private bool CourseProgramExists(int id)
        {
            return _context.CoursePrograms.Any(e => e.CourseProgramId == id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RetrieveFromGCAsync()
        {
            //reference to https://html-agility-pack.net/
            List<CourseProgram> pendingTitle = new List<CourseProgram>();
            List<string> comingData = new List<string>();
            //retrieve table info from Georgian College
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.georgiancollege.ca/academics/programs/");
                //curl - X HEAD - i https://www.georgiancollege.ca/academics/programs/
                //content - encoding = gzip
                request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                //get response
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //convert to string
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                string html = readStream.ReadToEnd();
                //pass html
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);
                var tr = doc.DocumentNode.SelectNodes("//tbody/tr");
                foreach (var el in tr)
                {
                    var tds = el.Elements("td");
                    string code = tds.ElementAt(0).Element("a").InnerText;
                    string title = tds.ElementAt(1).Element("a").InnerText;
                    comingData.Add(title);
                    /*Console.WriteLine($"Code: {code}, Title: {title}");
*/
                }
                //retriev from db
                List<CourseProgram> dbData = await _context.CoursePrograms.ToListAsync();
                List<string> dbTitle = new List<string>();
                foreach (var program in dbData)
                {
                    dbTitle.Add(program.Title);
                }
                //compare
                IEnumerable<string> newTitle = comingData.Except(dbTitle);

                if (newTitle.Count() > 0)
                {
                    //update db
                    foreach (string title in newTitle)
                    {
                        pendingTitle.Add(
                            new CourseProgram
                            {
                                Title = title
                            }); ;
                    }
                    _context.CoursePrograms.AddRange(pendingTitle);
                    await _context.SaveChangesAsync();
                }

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }


            return RedirectToAction("Index");
        }
    }
}
