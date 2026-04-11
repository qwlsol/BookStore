using System.ComponentModel.DataAnnotations;

namespace BookStore.Model
{
    public class Buyer : EFModel
    {
        [Required(ErrorMessage = "Строка 'Фамилия' обязательна к заполнению")]
        public string LastName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Строка 'Email' обязательна к заполнению")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Строка  'Телефон' обязательна к заполнению")]
        public string? Phone { get; set; }
        public List<Book>? Books { get; set; }
    }
}
