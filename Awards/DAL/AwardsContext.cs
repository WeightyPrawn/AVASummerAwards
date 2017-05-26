using Awards.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Awards.DAL
{
    public class AwardsContext : DbContext
    {
        public AwardsContext() : base("AwardsContext")
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Nomination> Nominations { get; set; }
        public DbSet<Nominee> Nominees { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}