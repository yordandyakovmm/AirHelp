namespace AirHelp.DAL.Migration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixDate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "CreateDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "CreateDate", c => c.DateTime(nullable: false));
        }
    }
}
