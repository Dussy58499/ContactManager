using Microsoft.EntityFrameworkCore;
using Repository.Models.Domain;

namespace Repository.Data
{
    public class ContactDbContext : DbContext
    {
        public ContactDbContext(DbContextOptions<ContactDbContext> options) : base(options) { }
        public DbSet<Contact> Contacts { get; set; }
    }
}