using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ulvi.ViewModels
{
    public class UpdateInvoiceVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public DateTime InvoiceDate { get; set; }

        [Required]
        public Decimal NetAmount { get; set; }

        [Required]
        public Decimal TaxAmount { get; set; }

        [Required]
        public Decimal TotalAmount { get; set; }

        [Required]
        public string InvoiceNote { get; set; }

        public int PaymentStatus { get; set; }

        public int InvoiceNumber { get; set; }

        public int ProjectId { get; set; }

        public int ClientId { get; set; }
    }
}
