using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Model
{
    public class Author : EFModel
    {
        public List<Book> Books { get; set; } = new List<Book>();
    }
}
