using BookStore.Data;
using BookStore.Hubs;
using BookStore.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Pages.Books
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<BookHub> _bookHubContext;

        public DeleteModel(ApplicationDbContext context, IHubContext<BookHub> bookHubContext)
        {
            _context = context;
            _bookHubContext = bookHubContext;
        }

        [BindProperty]
        public Book? Book { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Book = await _context.Books
                        .Where(c => c.Id == id)
                        .Include(b => b.Author)
                        .FirstOrDefaultAsync();

            if (Book == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var book = await _context.Books.FindAsync(Book.Id);

            if (book != null)
            {
                var bookId = book.Id;
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();

                var bookData = new
                {
                    id = bookId,
                    isDeleted = true
                };

                await _bookHubContext.Clients.All.SendAsync("BookUpdated", bookData);
            }

            return RedirectToPage("Index");
        }
    }
}