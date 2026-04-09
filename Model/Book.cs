namespace BookStore.Model
{
    public class Book : EFModel
    {
        public string Title { get; set; } = string.Empty;
        public Author Author { get; set; } = new();
        public int AuthorID { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}
