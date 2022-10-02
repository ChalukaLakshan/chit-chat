using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Chat.Data.EfCore
{
    public class ChitChatDbContext : DbContext
    {
        public ChitChatDbContext()
        {
        }

        public ChitChatDbContext(DbContextOptions<ChitChatDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();

            modelBuilder.Entity<Message>()
                .HasOne(m => m.FromUser)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

    }
}
