namespace AirHelp.DAL.Migration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserDocument : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Document");
            AlterColumn("dbo.Document", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Document", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Document");
            AlterColumn("dbo.Document", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Document", "Id");
        }
    }
}
