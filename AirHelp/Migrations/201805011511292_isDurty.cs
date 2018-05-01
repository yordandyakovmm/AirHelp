namespace AirHelp.DAL.Migration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isDurty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Claim", "isDurty", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Claim", "isDurty");
        }
    }
}
