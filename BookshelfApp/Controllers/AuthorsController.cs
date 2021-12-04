using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookshelfApp.Data;
using BookshelfApp.Models;

namespace BookshelfApp.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthorsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)

        {

            _context = context;
            _userManager = userManager;

        }


        // GET: Authors
        public async Task<ActionResult> Index(string searchString)
        {
            var user = await GetCurrentUserAsync();
            var authors = await _context.Author
                //.Where(ti => ti.ApplicationUserId == user.Id)
                .Include(tdi => tdi.ApplicationUser)
                .ToListAsync();

            if (searchString != null)
            {
                var filteredAuthors = _context.Author.Where(s => s.FirstName.Contains(searchString) || s.LastName.Contains(searchString) );
                return View(filteredAuthors);
            };

            return View(authors);
        }


        // GET: Authors/Details/1
        public async Task<ActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Author
               //.Where(p => p.UserId == user.Id)
               .Include(p => p.ApplicationUser)
               .FirstOrDefaultAsync(p => p.Id == id);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }



        // GET: Authors/Create
        public ActionResult Create()
        {
            return View();
        }


        // POST: Authors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Author author)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                author.ApplicationUserId = user.Id;

                _context.Author.Add(author);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }



        // GET: Authors/Edit/1
        public async Task<ActionResult> Edit(int id)
        {
            var author = await _context.Author.FirstOrDefaultAsync(p => p.Id == id);
            var loggedInUser = await GetCurrentUserAsync();

            if (author.ApplicationUserId != loggedInUser.Id)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Author author)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                author.ApplicationUserId = user.Id;

                _context.Author.Update(author);
                await _context.SaveChangesAsync();
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }



        // GET: Authors/Delete/1
        public async Task<ActionResult> Delete(int id)
        {
            var loggedInUser = await GetCurrentUserAsync();
            var author = await _context.Author
                .Include(p => p.ApplicationUser)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (author == null)
            {
                return NotFound();
            }

            if (author.ApplicationUserId != loggedInUser.Id)
            {
                return NotFound();
            }

            return View(author);
        }


        // POST: Authors/Delete/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                var author = await _context.Author.FindAsync(id);
                _context.Author.Remove(author);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

    }
}