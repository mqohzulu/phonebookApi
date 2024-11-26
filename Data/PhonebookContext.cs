using Microsoft.EntityFrameworkCore;
using phonebookApi.Models;

namespace PhonebookApp.API.Data
{
    public class PhonebookContext : DbContext
    {
        public PhonebookContext(DbContextOptions<PhonebookContext> options)
            : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Contact>()
                .HasIndex(c => c.PhoneNumber)
                .IsUnique();


            modelBuilder.Entity<Contact>().HasData(
                new Contact
                {
                    Id = 1,
                    Name = "John Langalibalele Dube",
                    PhoneNumber = "0721234567",
                    Email = "johnthan@example.com"
                },
                new Contact
                {
                    Id = 2,
                    Name = "Dorothy Smith",
                    PhoneNumber = "0829876543",
                    Email = "Dorry05@example.com"
                }
            );
        }
    }
}