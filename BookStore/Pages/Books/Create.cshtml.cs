using BookStore.Data;
using BookStore.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStore.Pages.Books
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Book Book { get; set; } = new();

        public SelectList AuthorList { get; set; }

        public void OnGet()
        {
            var authors = _context.Authors.ToList();
            AuthorList = new SelectList(authors, "Id", "Name");
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                var authors = _context.Authors.ToList();
                AuthorList = new SelectList(authors, "Id", "Name");
                return Page();
            }
            Book.Author = null;
            _context.Books.Add(Book);
            _context.SaveChanges();

            return RedirectToPage("Index");
        }
    }
}