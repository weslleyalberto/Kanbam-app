﻿using System.ComponentModel.DataAnnotations;

namespace Kabam.Domain
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
