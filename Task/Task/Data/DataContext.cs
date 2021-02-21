using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using Ulvi.Models;

namespace Ulvi.Data
{
    public class DataContext:IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Bio> Bios { get; set; }

    }
}