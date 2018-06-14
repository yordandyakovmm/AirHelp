namespace AirHelp.DAL.Migration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateidentity : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.AirPortData");
            AlterColumn("dbo.AirPortData", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.AirPortData", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.AirPortData");
            AlterColumn("dbo.AirPortData", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.AirPortData", "Id");
        }
    }
}
