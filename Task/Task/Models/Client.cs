using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ulvi.Models;

namespace Ulvi.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        public ICollection<Invoice> Invoices { get; set; }
    }
}