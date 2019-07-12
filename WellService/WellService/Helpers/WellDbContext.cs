using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WellService.Models;

namespace WellService.Helpers
{
 
        public class WellDbContext : DbContext
        {
            public DbSet<ClientQuota> ClientQuotas { get; set; }


            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite($"Filename={Constants.DBFileName}", options =>
                {
                    options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                });


                base.OnConfiguring(optionsBuilder);
            }


            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                // Map table names
                modelBuilder.Entity<ClientQuota>().ToTable("ClientQuotas", "Well");
                modelBuilder.Entity<ClientQuota>(entity =>
                {
                    entity.HasKey(e => e.ClientID);
                    entity.Property(e => e.ClientID).ValueGeneratedOnAdd();
                    entity.HasIndex(e => e.ClientName).IsUnique();
                    entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
                });


                base.OnModelCreating(modelBuilder);
            }
        }
    
}
