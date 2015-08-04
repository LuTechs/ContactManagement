using System;
using System.Data.Entity;

namespace ContactManagement.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("defaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Properties<DateTime>()
                .Configure(c => c.HasColumnType("datetime2"));
        }

        public DbSet<Contact> Contacts { get; set; }
    }
}