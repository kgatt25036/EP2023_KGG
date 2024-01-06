using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataContext
{
    public class AirlineDbContext : IdentityDbContext<CustomUser>
    {
        public AirlineDbContext(DbContextOptions<AirlineDbContext> options)
          : base(options)
        {
        }

        public DbSet<Flight> Flights { get; set; }
        public DbSet<Ticket> Tickets { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Flight>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Ticket>().Property(x => x.Id).HasDefaultValueSql("NEWID()");

        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
