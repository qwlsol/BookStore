using BookStore.Data;
using BookStore.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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
        public Book? Book { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Укажите автора")]
        public string AuthorName { get; set; } = string.Empty;
        public IActionResult OnGet(int id)
        {
            Book = _context.Books
                      .Where(c => c.Id == id)
                      .Include(b => b.Author)
                      .FirstOrDefault();

            if (Book == null)
                return NotFound();

            AuthorName = Book.Author?.Name ?? string.Empty;

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var author = _context.Authors.FirstOrDefault(a => a.Name == AuthorName);
            if (author == null)
            {
                author = new Author { Name = AuthorName };
                _context.Authors.Add(author);
                _context.SaveChanges();
            }

            Book.AuthorID = author.Id;
            Book.Author = author;

            _context.Books.Update(Book);
            _context.SaveChanges();

            return RedirectToPage("Index");
        }
    }
}
