
using System.Collections.Generic;

namespace Ulvi.ViewModels
{
    // For Admin Panel
    public class UserVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsActivated { get; set; }
        public List<string> Roles { get; set; }
        public bool IsArchived { get; set; }
    }
}
