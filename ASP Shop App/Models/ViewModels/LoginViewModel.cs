﻿using System.ComponentModel.DataAnnotations;

namespace ASP_Shop_App.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }



        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
