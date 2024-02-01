using FinalProjectFb.Domain.Entities.Common;
using FinalProjectFb.Web.Models;
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
    public class AppDbContext:IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) 
        {
            
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<News> Newss { get; set; }
        public DbSet<Image> Images { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.AppQueryFilter();
            //modelBuilder.Entity<Tag>().HasQueryFilter(c => c.IsDeleted == false);
            modelBuilder.Entity<Category>().Property(c => c.Name).IsRequired().HasMaxLength(25);
            modelBuilder.Entity<News>().Property(c => c.Name).IsRequired().HasMaxLength(25);
            modelBuilder.Entity<Image>().Property(c => c.Name).IsRequired().HasMaxLength(25);
            modelBuilder.Entity<Setting>().Property(c => c.Key).IsRequired().HasMaxLength(25);
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        //public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{
        //    var entities = ChangeTracker.Entries<BaseEntity>();

        //    foreach (var data in entities)
        //    {
        //        switch (data.State)
        //        {
        //            case EntityState.Modified:
        //                data.Entity.ModifiedAt = DateTime.Now;
        //                break;
        //            case EntityState.Added:
        //                data.Entity.CreatedAt = DateTime.Now;
        //                break;
        //        }
        //    }

        //    return base.SaveChangesAsync(cancellationToken);
        //}
    }
}
