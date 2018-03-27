namespace AirHelp.DAL.Migration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcompensationAmount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Claim", "CompensationAmount", c => c.Int(nullable: false));
            AddColumn("dbo.Claim", "CompensationReason", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Claim", "CompensationReason");
            DropColumn("dbo.Claim", "CompensationAmount");
        }
    }
}
