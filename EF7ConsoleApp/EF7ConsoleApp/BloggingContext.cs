using System;
using Microsoft.Data.Entity;
using System.Collections.Generic;
using ServiceCruiser.Model.Entities.Contracts;
using ServiceCruiser.Model.Entities.FieldServices;

namespace EF7ConsoleApp
{
    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<AppointmentWindow> WorkOrders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename=Blogging.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Make Blog.Url required
            modelBuilder.Entity<Blog>()
                .Property(b => b.Url)
                .IsRequired();
        }
    }

}
