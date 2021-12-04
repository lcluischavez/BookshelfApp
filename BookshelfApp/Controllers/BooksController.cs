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
    public class BooksController : Controller
    { 
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BooksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)

        {

            _context = context;
            _userManager = userManager;

        }


        // GET: Cars
        public async Task<ActionResult> Index(string searchString)
        {
            var user = await GetCurrentUserAsync();
            var books = await _context.Book
                //.Where(ti => ti.ApplicationUserId == user.Id)
                .Include(tdi => tdi.ApplicationUser)
                .ToListAsync();

            if (searchString != null)
            {
                var filteredBooks = _context.Book.Where(s => s.Title.Contains(searchString) /* || s.ISBN.Contains(searchString)*/ ); 
                return View(filteredBooks);
            };

            return View(books);
        }


        // GET: Books/Details/1
        public async Task<ActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
               //.Where(p => p.UserId == user.Id)
               .Include(p => p.ApplicationUser)
               .FirstOrDefaultAsync(p => p.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }



        // GET: Books/Create
        public ActionResult Create()
        {
            return View();
        }


        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Book book)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                book.ApplicationUserId = user.Id;

                _context.Book.Add(book);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }



        // GET: Books/Edit/1
        public async Task<ActionResult> Edit(int id)
        {
            var book = await _context.Book.FirstOrDefaultAsync(p => p.Id == id);
            var loggedInUser = await GetCurrentUserAsync();

            if (book.ApplicationUserId != loggedInUser.Id)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Book book)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                book.ApplicationUserId = user.Id;

                _context.Book.Update(book);
                await _context.SaveChangesAsync();
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }



        // GET: Books/Delete/1
        public async Task<ActionResult> Delete(int id)
        {
            var loggedInUser = await GetCurrentUserAsync();
            var book = await _context.Book
                .Include(p => p.ApplicationUser)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            if (book.ApplicationUserId != loggedInUser.Id)
            {
                return NotFound();
            }

            return View(book);
        }


        // POST: Books/Delete/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                var book = await _context.Book.FindAsync(id);
                _context.Book.Remove(book);
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