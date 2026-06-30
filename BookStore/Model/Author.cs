using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Model
{
    public class Author : EFModel
    {
        [Required(ErrorMessage = "Имя автора обязательно")]
        public override string? Name { get; set; }
        public List<Book> Books { get; set; } = new List<Book>();
    }
}
