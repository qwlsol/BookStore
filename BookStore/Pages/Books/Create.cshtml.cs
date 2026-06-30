using BookStore.Data;
using BookStore.Model;
using BookStore.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Pages.Books
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<BookHub>? _bookHubContext;

        // Только ОДИН конструктор
        public CreateModel(ApplicationDbContext context, IHubContext<BookHub> bookHubContext)
        {
            _context = context;
            _bookHubContext = bookHubContext;
        }

        [BindProperty]
        public Book Book { get; set; } = new();

        public SelectList? AuthorList { get; set; }

        public void OnGet()
        {
            var authors = _context.Authors.ToList();
            AuthorList = new SelectList(authors, "Id", "Name");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var authors = _context.Authors.ToList();
                AuthorList = new SelectList(authors, "Id", "Name");
                return Page();
            }

            Book.Author = null;
            _context.Books.Add(Book);
            await _context.SaveChangesAsync();

            var bookWithAuthor = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.Id == Book.Id);

            if (bookWithAuthor != null && _bookHubContext != null)
            {
                var bookData = new
                {
                    id = bookWithAuthor.Id,
                    title = bookWithAuthor.Title,
                    price = bookWithAuthor.Price,
                    quantity = bookWithAuthor.Quantity,
                    authorName = bookWithAuthor.Author?.Name ?? ""
                };

                await _bookHubContext.Clients.All.SendAsync("BookUpdated", bookData);
            }

            return RedirectToPage("Index");
        }
    }
}