using BookStore.Data;
using BookStore.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

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
        public Book Book { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Требуется ввести автора")]
        public string AuthorName { get; set; } = string.Empty;

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            // Найти или создать автора
            var author = _context.Authors.FirstOrDefault(a => a.Name == AuthorName);
            if (author == null)
            {
                author = new Author { Name = AuthorName };
                _context.Authors.Add(author);
                _context.SaveChanges();
            }

            Book.AuthorID = author.Id;
            Book.Author = author;

            _context.Books.Add(Book);
            _context.SaveChanges();

            return RedirectToPage("Index");
        }
    }
}
