﻿using System.ComponentModel.DataAnnotations;

namespace Ulvi.ViewModels
{
    // For Login
    public class LoginVM
    {
        [Required, EmailAddress, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
