using Microsoft.EntityFrameworkCore;
using MyPortfolio.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyPortfolio.Infrastructure
{
    public class DataContext : DbContext
    {
        //passing dbcontext options to the base type
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //assign new incremental guid to the id field on adding new records
            modelBuilder.Entity<Owner>().Property(i => i.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<PortfolioItem>().Property(i => i.Id).HasDefaultValueSql("NEWID()");

            //Db Initializer
            //Should use a separate fully-implemented DBInitializer
            modelBuilder.Entity<Owner>().HasData(
                new Owner()
                {
                    Id = Guid.NewGuid(),
                    AvatarUrl = "Avatar.jpg",
                    FullName = "Abdulrahman Seliem",
                    ProfileNote = "Senior Full-Stack Developer" 
                }) ;
        }

        public DbSet<Owner> Owners { get; set; }
        //We dont have to specify a dbset for addresses
        //It is present relational to PortfolioItems
        public DbSet<PortfolioItem> PortfolioItems { get; set; }
    }
}
