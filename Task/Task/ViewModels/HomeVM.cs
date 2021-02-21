using System.Collections.Generic;
using Ulvi.Models;

namespace Ulvi.ViewModels
{
    public class HomeVM
    {
        public User User { get; set; }
        public ICollection<Invoice> Invoices { get; set; }
    }
}