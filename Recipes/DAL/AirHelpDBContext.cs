using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Recipes.DAL
{
	
    public class AirHelpDBContext : DbContext
    {

        public AirHelpDBContext() : base("AirHelpConnectionString")
        {
            Database.SetInitializer<AirHelpDBContext>(null);
        }

        public DbSet<Users> Users { get; set; }

        //public DbSet<Form> Forms { get; set; }
        //public DbSet<Measure> Measures { get; set; }
        //public DbSet<Study> Studys { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }

}
