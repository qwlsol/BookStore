using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookStore.Data;
using BookStore.Model;

namespace BookStore.Pages.Books
{
    public class IndexModel : PageModel

    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _context;

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public List<Book> Books { get; set; }

        public void OnGet()
        {
            Books = _context.Books.ToList();
        }
    }
}
