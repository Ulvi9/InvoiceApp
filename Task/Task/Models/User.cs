using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Ulvi.Models
{
    public class User:IdentityUser
    {
        [Required, MaxLength(20)]
        public string Name { get; set; }

        public bool IsActivated { get; set; }
    }
}
