namespace ASP_Shop_App.Models
{
    public class Order:BaseEntity
    {
        public bool IsOrdered { get; set; } = false;
        public AppUser User { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public ICollection<Product> Products { get; set; } = new List<Product>();

    }
}
