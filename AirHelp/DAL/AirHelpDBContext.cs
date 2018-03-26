using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace AirHelp.DAL
{
	
    public class AirHelpDBContext : DbContext
    {

        public AirHelpDBContext() : base("AirHelpConnectionString")
        {
            Database.SetInitializer<AirHelpDBContext>(null);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<AdditionalUser> AdditionalUsers { get; set; }
        public DbSet<AirPort> AirPorts { get; set; }
        public DbSet<Document> Documents { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }

}
