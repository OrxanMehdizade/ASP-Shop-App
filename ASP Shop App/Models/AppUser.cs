using Microsoft.AspNetCore.Identity;

namespace ASP_Shop_App.Models
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; }=null!;
        public int ordersID { get; set; }
        public Order orders { get; set; }
    }
}
