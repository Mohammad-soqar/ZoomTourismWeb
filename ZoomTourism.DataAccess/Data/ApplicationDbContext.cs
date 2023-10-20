using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using ZoomTourism.Models;

namespace ZoomTourism.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarImages> CarImages { get; set; }
        public DbSet<Lead> Leads { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ReviewLink> ReviewLinks { get; set; }
        public DbSet<ATask> Tasks { get; set; }

    }
}
