using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookshelfApp.Data;
using BookshelfApp.Models;
using BookshelfApp.Models.ViewModels;

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


        // GET: Books
        public async Task<ActionResult> Index(string searchString)
        {
            var user = await GetCurrentUserAsync();
            var books = await _context.Book
                //.Where(ti => ti.ApplicationUserId == user.Id)
                .Include(tdi => tdi.ApplicationUser)
                .Include(tdi => tdi.Genre)
                .ToListAsync();

            if (searchString != null)
            {
                var filteredBooks = _context.Book.Where(s => s.Title.Contains(searchString));
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

            var bookk = await _context.Book
               //.Where(p => p.UserId == user.Id)
               .Include(p => p.ApplicationUser)
               .Include(p => p.Genre)
               .FirstOrDefaultAsync(p => p.Id == id);

            if (bookk == null)
            {
                return NotFound();
            }

            return View(bookk);
        }



        // GET: Books/Create
        public async Task<ActionResult> Create()
        {
            var genreOptions = await _context.Genre.Select(e => new SelectListItem()
            {
                Text = e.Name,
                Value = e.Id.ToString()
            })
                .ToListAsync();
            var viewModel = new BookFormViewModel();
            viewModel.GenreOptions = genreOptions;
            return View(viewModel);
        }


        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BookFormViewModel bookFormView)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                var books = new Book()
                {
                    Title = bookFormView.Title,
                    Author = bookFormView.Author,
                    ISBN = bookFormView.ISBN,
                    PublishDate = bookFormView.PublishDate,
                    ApplicationUserId = user.Id,
                    GenreId = bookFormView.GenreId,
                };


                _context.Book.Add(books);
                await _context.SaveChangesAsync();

                // TODO: Add insert logic here

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
            var loggedInUser = await GetCurrentUserAsync();
            var genres = await _context.Genre.Select(e => new SelectListItem()
            {
                Text = e.Name,
                Value = e.Id.ToString()
            })
               .ToListAsync();

            var bookk = await _context.Book.FirstOrDefaultAsync(e => e.Id == id);
            var viewModel = new BookFormViewModel()
            {
                Title = bookk.Title,
                Author = bookk.Author,
                ISBN = bookk.ISBN,
                PublishDate = bookk.PublishDate,
                GenreId = bookk.GenreId,
                GenreOptions = genres,
            };

            if (bookk.ApplicationUserId != loggedInUser.Id)
            {
                return NotFound();
            }

            return View(viewModel);
        }


        // POST: Books/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, BookFormViewModel bookFormView)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                var books = new Book()
                {
                    Id = id,
                    Title = bookFormView.Title,
                    Author = bookFormView.Author,
                    ISBN = bookFormView.ISBN,
                    PublishDate = bookFormView.PublishDate,
                    ApplicationUserId = user.Id,
                    GenreId = bookFormView.GenreId,
                };

                _context.Book.Update(books);
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

            var bookk = await _context.Book
                .Include(e => e.ApplicationUser)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (bookk.ApplicationUserId != loggedInUser.Id)
            {
                return NotFound();
            }

            return View(bookk);
        }


        // POST: Books/Delete/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                //TODO: Add delete logic here
                var bookk = await _context.Book.FindAsync(id);
                _context.Book.Remove(bookk);
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