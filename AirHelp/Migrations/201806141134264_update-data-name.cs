namespace AirHelp.DAL.Migration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedataname : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.AirPortData", new[] { "name" });
            AlterColumn("dbo.AirPortData", "name", c => c.String(maxLength: 100, unicode: false));
            CreateIndex("dbo.AirPortData", "name");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AirPortData", new[] { "name" });
            AlterColumn("dbo.AirPortData", "name", c => c.String(maxLength: 60, unicode: false));
            CreateIndex("dbo.AirPortData", "name");
        }
    }
}
