using ContactManagement.Domain.Entities;
using ContactManagement.Domain.ValueObjects;
using ContactManagement.Infrastructure.Persistance.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Infrastructure.Persistance
{

    public class PhonebookContext : DbContext
    {
        public PhonebookContext(DbContextOptions<PhonebookContext> options): base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ContactConfiguration());

            // Seed data using domain entities and value objects
            var defaultUserId = Guid.Parse("A7BF0806-0C14-4983-8D3D-A6BE424DC269"); 

            // Create value objects for seeding
            var johnEmail = Email.Create("johnthan@example.com").Value;
            var johnPhone = PhoneNumber.Create("0721234567").Value;
            var johnAddress = Address.Create("123 Main St", "Johannesburg", "Gauteng", "2000", "South Africa").Value;

            var dorothyEmail = Email.Create("Dorry05@example.com").Value;
            var dorothyPhone = PhoneNumber.Create("0829876543").Value;
            var dorothyAddress = Address.Create("456 Oak Ave", "Cape Town", "Western Cape", "8000", "South Africa").Value;

            // Create contact entities
            var contact1 = Contact.Create(
                "John",
                "Langalibalele Dube",
                johnEmail.Value,
                johnPhone.Value,
                johnAddress,
                defaultUserId).Value;

            var contact2 = Contact.Create(
                "Dorothy",
                "Smith",
                dorothyEmail.Value,
                dorothyPhone.Value,
                dorothyAddress,
                defaultUserId).Value;

            // Configure seed data
            modelBuilder.Entity<Contact>().HasData(
                new
                {
                    contact1.Id,
                    contact1.FirstName,
                    contact1.LastName,
                    Email = contact1.Email.Value,
                    PhoneNumber = contact1.PhoneNumber.Value,
                    Street = contact1.Address.Street,
                    City = contact1.Address.City,
                    State = contact1.Address.State,
                    PostalCode = contact1.Address.PostalCode,
                    Country = contact1.Address.Country,
                    contact1.Status,
                    contact1.UserId,
                    contact1.CreatedAt,
                    contact1.UpdatedAt
                },
                new
                {
                    contact2.Id,
                    contact2.FirstName,
                    contact2.LastName,
                    Email = contact2.Email.Value,
                    PhoneNumber = contact2.PhoneNumber.Value,
                    Street = contact2.Address.Street,
                    City = contact2.Address.City,
                    State = contact2.Address.State,
                    PostalCode = contact2.Address.PostalCode,
                    Country = contact2.Address.Country,
                    contact2.Status,
                    contact2.UserId,
                    contact2.CreatedAt,
                    contact2.UpdatedAt
                }
            );
        }
    }
}
