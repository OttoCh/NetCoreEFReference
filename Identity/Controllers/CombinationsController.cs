using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Identity.Data;
using Identity.Models;

namespace Identity.Controllers
{
    public class CombinationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CombinationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Combinations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Combination.ToListAsync());
        }

        // GET: Combinations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var combination = await _context.Combination
                .Include("CombinationNumber")
                .SingleOrDefaultAsync(m => m.CombinationId == id);
            if (combination == null)
            {
                return NotFound();
            }

            return View(combination);
        }

        // GET: Combinations/Create
        public IActionResult Create()
        {
            return View();
        }

        public string Join(int id)
        {
            var query = from s in _context.Content
                        join r in _context.Combination on s.Id equals r.CombinationId
                        select new
                        {
                            Name = s.ContentName,
                            Id = r.CombinationId
                        };

            var res = query.ToList();

            foreach(var i in res)
            {
                string n = i.Name;
                int nid = i.Id;
            }

            var query2 = from s in _context.Combination.Include("CombinationNumber")
                         select s;

            var res2 = query2.ToList();

            return "success";

        }

        // POST: Combinations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("CombinationId,Name")] Combination combination)
        public async Task<IActionResult> Create(Combination combination)
        {
            if (ModelState.IsValid)
            {
                _context.Add(combination);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(combination);
        }

        // GET: Combinations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //to include 
            var combination = await _context.Combination
                .Include("CombinationNumber")
                .SingleOrDefaultAsync(m => m.CombinationId == id);

            //unable to access foreign key using linq, above line already use Lazy Load so it doesnt matter
            ////using linq query to get all from context
            ////var query = from i in _context.Content where i == id select i;
            ////foreach (Content i in query)
            ////{
            ////    combination.CombinationNumber.Add(i);
            ////}

            ////using lambda to get all from context
            ////var content = _context.Content.Where(o => o.ContentName.Equals("A")).FirstOrDefaultAsync();
            ////foreach (Content i in content)
            ////{
            ////    combination.CombinationNumber.Add(i);
            ////}

            if (combination == null)
            {
                return NotFound();
            }
            return View(combination);
        }

        // POST: Combinations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Combination combination)
        {
            if (id != combination.CombinationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //the problem when updating is because the binded model doesnt have proper id, it solved by editing the cshtml to include hidden field for id

                    ////how to update?
                    ////_context.Content.Where(item => );
                    ////_context.Content.Update(combination.CombinationNumber.ElementAt<Content>(0));

                    ////update combination table only, removing insert values to content table
                    ////combination.CombinationNumber = null;

                    _context.Combination.Update(combination);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CombinationExists(combination.CombinationId))
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
            return View(combination);
        }

        // GET: Combinations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var combination = await _context.Combination
                .Include("CombinationNumber")
                .SingleOrDefaultAsync(m => m.CombinationId == id);
            if (combination == null)
            {
                return NotFound();
            }

            return View(combination);
        }

        // POST: Combinations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //find and delete combination
            var combination = await _context.Combination
                .Include("CombinationNumber")
                .SingleOrDefaultAsync(m => m.CombinationId == id);
            
            _context.Combination.Remove(combination);

            //find and delete content
            List<Content> allRemovedContent = new List<Content>();
            foreach (Content c in combination.CombinationNumber)
            {
                var content = await _context.Content
                    .SingleOrDefaultAsync(n => n.Id == c.Id);
                allRemovedContent.Add(content);
                //_context.Content.Remove(content);
            }

            _context.Content.RemoveRange(allRemovedContent);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CombinationExists(int id)
        {
            return _context.Combination.Any(e => e.CombinationId == id);
        }
    }
}
