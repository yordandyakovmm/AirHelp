namespace AirHelp.DAL.Migration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class increacemanelength : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.AirPortData", new[] { "name" });
            DropPrimaryKey("dbo.AirPortData");
            AlterColumn("dbo.AirPortData", "Id", c => c.Int(nullable: false));
            AlterColumn("dbo.AirPortData", "name", c => c.String(maxLength: 60, unicode: false));
            AddPrimaryKey("dbo.AirPortData", "Id");
            CreateIndex("dbo.AirPortData", "name");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AirPortData", new[] { "name" });
            DropPrimaryKey("dbo.AirPortData");
            AlterColumn("dbo.AirPortData", "name", c => c.String(maxLength: 40, unicode: false));
            AlterColumn("dbo.AirPortData", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.AirPortData", "Id");
            CreateIndex("dbo.AirPortData", "name");
        }
    }
}
