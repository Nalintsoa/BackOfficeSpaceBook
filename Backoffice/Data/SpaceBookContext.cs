using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Backoffice.Models;

namespace Backoffice.Data
{
    public class SpaceBookContext : DbContext
    {
        public SpaceBookContext (DbContextOptions<SpaceBookContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingEquip> BookingEquips { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Space> Spaces { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Booking>().ToTable("Booking");
            modelBuilder.Entity<BookingEquip>().ToTable("BookingEquip");
            modelBuilder.Entity<Equipment>().ToTable("Equipment");
            modelBuilder.Entity<Space>().ToTable("Space");
        }
    }
}
