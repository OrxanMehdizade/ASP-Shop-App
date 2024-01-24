namespace ASP_Shop_App.Models.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public List<OrProViewModel> Prodcuts { get; set; }
    }
}
