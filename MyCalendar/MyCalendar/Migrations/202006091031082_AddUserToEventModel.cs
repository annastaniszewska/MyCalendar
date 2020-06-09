namespace MyCalendar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserToEventModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Events", "User_Id");
            AddForeignKey("dbo.Events", "User_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Events", new[] { "User_Id" });
            DropColumn("dbo.Events", "User_Id");
        }
    }
}
