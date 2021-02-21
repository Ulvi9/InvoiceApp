using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ulvi.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Invoice> Invoices { get; set; }
    }
}