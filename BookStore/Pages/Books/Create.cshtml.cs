using BookStore.Data;
using BookStore.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        [Required(ErrorMessage = "Выберите автора")]
        public int SelectedAuthorId { get; set; }

        public SelectList AuthorList { get; set; }

        public void OnGet()
        {
            LoadAuthorList();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                LoadAuthorList();
                return Page();
            }

            Book.AuthorID = SelectedAuthorId;
            _context.Books.Add(Book);
            _context.SaveChanges();

            return RedirectToPage("Index");
        }

        private void LoadAuthorList()
        {
            var authors = _context.Authors.ToList();
            AuthorList = new SelectList(authors, "Id", "Name");
        }
    }
}