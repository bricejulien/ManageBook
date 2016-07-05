using ManageBookModels;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ManageBook.Models
{
    public class ManageBookContext : DbContext
    {
        public virtual DbSet<Entry> Entries { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public ManageBookContext()
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(x => x.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(x => x.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(x => new { x.RoleId, x.UserId });
            modelBuilder.Entity<Entry>().HasKey<int>(x => x.Id);
            modelBuilder.Entity<Project>().HasKey<int>(x => x.Id);
        }

        public System.Data.Entity.DbSet<ManageBookModels.Contact> Contacts { get; set; }
    }
}