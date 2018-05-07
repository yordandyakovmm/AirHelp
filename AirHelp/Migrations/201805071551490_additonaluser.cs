namespace AirHelp.DAL.Migration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class additonaluser : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.AdditionalUser");
            AlterColumn("dbo.AdditionalUser", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.AdditionalUser", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.AdditionalUser");
            AlterColumn("dbo.AdditionalUser", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.AdditionalUser", "Id");
        }
    }
}
