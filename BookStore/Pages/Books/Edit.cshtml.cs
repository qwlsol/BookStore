using BookStore.Data;
using BookStore.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Pages.Books
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Book Book { get; set; } = new();

        public SelectList AuthorList { get; set; }

        public IActionResult OnGet(int id)
        {
            Book = _context.Books
                .Where(b => b.Id == id)
                .Include(b => b.Author)
                .FirstOrDefault();

            if (Book == null)
                return NotFound();

            var authors = _context.Authors.ToList();
            AuthorList = new SelectList(authors, "Id", "Name", Book.AuthorID);

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                var authors = _context.Authors.ToList();
                AuthorList = new SelectList(authors, "Id", "Name", Book.AuthorID);
                return Page();
            }

            var bookToUpdate = _context.Books.Find(Book.Id);
            if (bookToUpdate == null)
                return NotFound();

            bookToUpdate.Title = Book.Title;
            bookToUpdate.AuthorID = Book.AuthorID;
            bookToUpdate.Price = Book.Price;
            bookToUpdate.Quantity = Book.Quantity;

            _context.SaveChanges();

            return RedirectToPage("Index");
        }
    }
}