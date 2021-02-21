using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ulvi.Models;

namespace Ulvi.ViewModels
{
    public class UserRoleVM
    {
        
            public User User { get; set; }
            public IList<string> UserRoles { get; set; }
        
    }
}
