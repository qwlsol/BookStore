using BookStore.Data;
using BookStore.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Pages.Books
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Book? Book { get; set; }

        public IActionResult OnGet(int id)
        {
            Book = _context.Books
                        .Where(c => c.Id == id)
                        .Include(b => b.Author)
                        .FirstOrDefault();

            if (Book == null)
                return NotFound();

            return Page();
        }
    }
}
