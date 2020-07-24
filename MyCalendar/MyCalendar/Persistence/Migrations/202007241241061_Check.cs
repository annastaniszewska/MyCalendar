namespace MyCalendar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Check : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Types", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Types", "Name", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
