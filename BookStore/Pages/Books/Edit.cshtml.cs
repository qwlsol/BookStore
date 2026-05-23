using BookStore.Data;
using BookStore.Hubs;
using BookStore.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Pages.Books
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<BookHub> _bookHubContext;

        public EditModel(ApplicationDbContext context, IHubContext<BookHub> bookHubContext)
        {
            _context = context;
            _bookHubContext = bookHubContext;
        }

        [BindProperty]
        public Book Book { get; set; } = new();

        public SelectList AuthorList { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Book = await _context.Books
                .Where(b => b.Id == id)
                .Include(b => b.Author)
                .FirstOrDefaultAsync();

            if (Book == null)
                return NotFound();

            var authors = _context.Authors.ToList();
            AuthorList = new SelectList(authors, "Id", "Name", Book.AuthorID);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var authors = _context.Authors.ToList();
                AuthorList = new SelectList(authors, "Id", "Name", Book.AuthorID);
                return Page();
            }

            var bookToUpdate = await _context.Books.FindAsync(Book.Id);
            if (bookToUpdate == null)
                return NotFound();

            bookToUpdate.Title = Book.Title;
            bookToUpdate.AuthorID = Book.AuthorID;
            bookToUpdate.Price = Book.Price;
            bookToUpdate.Quantity = Book.Quantity;

            await _context.SaveChangesAsync();

            var updatedBook = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.Id == Book.Id);

            var bookData = new
            {
                id = updatedBook.Id,
                title = updatedBook.Title,
                price = updatedBook.Price,
                quantity = updatedBook.Quantity,
                authorName = updatedBook.Author?.Name ?? ""
            };

            await _bookHubContext.Clients.All.SendAsync("BookUpdated", bookData);

            return RedirectToPage("Index");
        }
    }
}