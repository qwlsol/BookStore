using BookStore.Data;
using BookStore.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Pages.Authors
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public IActionResult OnPost()
        {
            var author = _context.Authors.Find(Author.Id);

            if (author != null)
            {
                _context.Authors.Remove(author);
                _context.SaveChanges();
            }

            return RedirectToPage("Index");
        }
    }
}