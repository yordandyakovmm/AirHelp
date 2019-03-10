namespace AirHelp.DAL.Migration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteClaim : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Claim", "isDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Claim", "isDeleted");
        }
    }
}
