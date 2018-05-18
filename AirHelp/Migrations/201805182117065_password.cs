namespace AirHelp.DAL.Migration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class password : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Claim", "isDurtyAdmin", c => c.Boolean(nullable: false));
            AddColumn("dbo.Claim", "isDurtyuser", c => c.Boolean(nullable: false));
            AddColumn("dbo.User", "changePasswordCode", c => c.String());
            AddColumn("dbo.User", "changePasswordCodeValudation", c => c.DateTime(nullable: false));
            DropColumn("dbo.Claim", "isDurty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Claim", "isDurty", c => c.Boolean(nullable: false));
            DropColumn("dbo.User", "changePasswordCodeValudation");
            DropColumn("dbo.User", "changePasswordCode");
            DropColumn("dbo.Claim", "isDurtyuser");
            DropColumn("dbo.Claim", "isDurtyAdmin");
        }
    }
}
