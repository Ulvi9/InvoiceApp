using System;
using System.ComponentModel.DataAnnotations;
using Ulvi.Enum;

namespace Ulvi.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime InvoiceDate { get; set; }

        public Decimal NetAmount { get; set; }

        public Decimal TaxAmount { get; set; }

        public Decimal TotalAmount { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public string InvoiceNote { get; set; }

        public int InvoiceNumber { get; set; }
        public bool IsDeleted { get; set; }
        public int ProjectId { get; set; }

        public Project Project { get; set; }

        public int ClientId { get; set; }

        public Client Client { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
    }
}