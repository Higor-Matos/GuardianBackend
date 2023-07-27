using Microsoft.EntityFrameworkCore;
using GuardianBackend.Domain.Entities;
using System.Collections.Generic;

namespace GuardianBackend.Infrastructure.Data
{
    public class GuardianDbContext : DbContext
    {
        public GuardianDbContext(DbContextOptions<GuardianDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
    }
}
