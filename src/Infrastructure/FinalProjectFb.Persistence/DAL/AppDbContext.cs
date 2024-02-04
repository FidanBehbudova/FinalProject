﻿using FinalProjectFb.Domain.Entities;
using FinalProjectFb.Domain.Entities.Common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Persistence.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<BasicFunctionsİnfo> BasicFunctionsİnfos { get; set; }
        public DbSet<Requirementsİnfo> Requirementsİnfos { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<CompanyCity> CompanyCities { get; set; }
        public DbSet<City> Cities { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Company>()
            .HasOne(c => c.Image)
            .WithOne(i => i.Company)
            .HasForeignKey<Image>(i => i.CompanyId);
            modelBuilder.Entity<CompanyCity>()
            .HasKey(cc => new { cc.CompanyId, cc.CityId });

            modelBuilder.Entity<CompanyCity>()
                .HasOne(cc => cc.Company)
                .WithMany(c => c.CompanyCities)
                .HasForeignKey(cc => cc.CompanyId);

            modelBuilder.Entity<CompanyCity>()
                .HasOne(cc => cc.City)
                .WithMany(c => c.CompanyCities)
                .HasForeignKey(cc => cc.CityId);

            modelBuilder.Entity<Job>()
                .HasOne(j => j.Image)
                .WithOne(i => i.Job)
                .HasForeignKey<Image>(i => i.JobId);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entities = ChangeTracker.Entries<BaseEntity>();

            foreach (var data in entities)
            {
                switch (data.State)
                {
                    case EntityState.Modified:
                        data.Entity.ModifiedAt = DateTime.Now;
                        break;
                    case EntityState.Added:
                        data.Entity.CreatedAt = DateTime.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
