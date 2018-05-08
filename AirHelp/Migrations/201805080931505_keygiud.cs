namespace AirHelp.DAL.Migration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class keygiud : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.AdditionalUser");
            AddColumn("dbo.AdditionalUser", "AdditionalUserId", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.AdditionalUser", "AdditionalUserId");
            DropColumn("dbo.AdditionalUser", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AdditionalUser", "Id", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.AdditionalUser");
            DropColumn("dbo.AdditionalUser", "AdditionalUserId");
            AddPrimaryKey("dbo.AdditionalUser", "Id");
        }
    }
}
