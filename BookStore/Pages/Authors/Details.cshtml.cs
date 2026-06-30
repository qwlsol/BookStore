using BookStore.Data;
using BookStore.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Pages.Authors
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Author Author { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            Author = _context.Authors
                .Include(a => a.Books)
                .FirstOrDefault(a => a.Id == id);

            if (Author == null)
                return NotFound();

            return Page();
        }
    }
}