namespace BookStore.Model
{
    public class Book : EFModel
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public decimal Price { get; set; }

    }
}
