namespace AirHelp.DAL.Migration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remane : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Claim", "CancelOverbokingDelay", c => c.Int(nullable: false));
            DropColumn("dbo.Claim", "CancelDelay");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Claim", "CancelDelay", c => c.Int(nullable: false));
            DropColumn("dbo.Claim", "CancelOverbokingDelay");
        }
    }
}
