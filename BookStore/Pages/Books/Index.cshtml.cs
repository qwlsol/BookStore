using BookStore.Data;
using BookStore.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Pages.Books
{
    [Authorize]
    public class IndexModel : PageModel

    {
        
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
           
            _context = context;
        }
        public List<Book> Books { get; set; }

        public void OnGet()
        {
            Books = _context.Books
                .Include(b => b.Author)
                .ToList();
        }
    }
}
