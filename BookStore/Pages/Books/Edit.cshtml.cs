using BookStore.Data;
using BookStore.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStore.Pages.Books
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Book? Book { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Выберите автора")]
        public int SelectedAuthorId { get; set; }

        public SelectList AuthorList { get; set; }

        public IActionResult OnGet(int id)
        {
            Book = _context.Books
                      .Where(c => c.Id == id)
                      .Include(b => b.Author)
                      .FirstOrDefault();

            if (Book == null)
                return NotFound();

            SelectedAuthorId = Book.AuthorID;
            LoadAuthorList();

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                LoadAuthorList();
                return Page();
            }

            var bookToUpdate = _context.Books.Find(Book.Id);
            if (bookToUpdate == null)
                return NotFound();

            bookToUpdate.Title = Book.Title;
            bookToUpdate.Price = Book.Price;
            bookToUpdate.Quantity = Book.Quantity;
            bookToUpdate.AuthorID = SelectedAuthorId;

            _context.Books.Update(bookToUpdate);
            _context.SaveChanges();

            return RedirectToPage("Index");
        }

        private void LoadAuthorList()
        {
            var authors = _context.Authors.ToList();
            AuthorList = new SelectList(authors, "Id", "Name", SelectedAuthorId);
        }
    }
}