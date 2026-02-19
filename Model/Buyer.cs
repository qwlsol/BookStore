namespace BookStore.Model
{
    public class Buyer : EFModel
    {
        public string LastName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
