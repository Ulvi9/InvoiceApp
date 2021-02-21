
using System.ComponentModel.DataAnnotations;

namespace Ulvi.ViewModels
{
    // For Register
    public class RegisterVM
    {
        [Required, StringLength(50)]
        public string Name { get; set; }
        [Required, StringLength(50)]
        public string UserName { get; set; }
        [Required, EmailAddress, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}