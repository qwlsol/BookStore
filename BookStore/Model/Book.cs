using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookStore.Model
{
    public class Book : EFModel
    {
        [Required(ErrorMessage = "Требуется ввести название")]
        public string Title { get; set; } = string.Empty;
        public Author Author { get; set; } = new();
        public int AuthorID { get; set; }
        [Range(1, 1000000, ErrorMessage = "Цена должна быть от 1 до 1000000")]
        public int Price { get; set; }
        [Range(1, 10000, ErrorMessage = "Количество должно быть от 1 до 10000")]
        public int Quantity { get; set; }
    }
}
