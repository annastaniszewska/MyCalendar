namespace MyCalendar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OverrideConventionsForEventsAndTypes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Events", "Type_Id", "dbo.Types");
            DropIndex("dbo.Events", new[] { "Type_Id" });
            AlterColumn("dbo.Events", "Type_Id", c => c.Byte(nullable: false));
            AlterColumn("dbo.Types", "Name", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.Events", "Type_Id");
            AddForeignKey("dbo.Events", "Type_Id", "dbo.Types", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "Type_Id", "dbo.Types");
            DropIndex("dbo.Events", new[] { "Type_Id" });
            AlterColumn("dbo.Types", "Name", c => c.String());
            AlterColumn("dbo.Events", "Type_Id", c => c.Byte());
            CreateIndex("dbo.Events", "Type_Id");
            AddForeignKey("dbo.Events", "Type_Id", "dbo.Types", "Id");
        }
    }
}
