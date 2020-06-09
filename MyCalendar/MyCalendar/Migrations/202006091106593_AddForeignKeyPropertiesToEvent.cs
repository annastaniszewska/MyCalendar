namespace MyCalendar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForeignKeyPropertiesToEvent : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Events", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Events", new[] { "User_Id" });
            RenameColumn(table: "dbo.Events", name: "Type_Id", newName: "TypeId");
            RenameColumn(table: "dbo.Events", name: "User_Id", newName: "UserId");
            RenameIndex(table: "dbo.Events", name: "IX_Type_Id", newName: "IX_TypeId");
            AlterColumn("dbo.Events", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Events", "UserId");
            AddForeignKey("dbo.Events", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Events", new[] { "UserId" });
            AlterColumn("dbo.Events", "UserId", c => c.String(maxLength: 128));
            RenameIndex(table: "dbo.Events", name: "IX_TypeId", newName: "IX_Type_Id");
            RenameColumn(table: "dbo.Events", name: "UserId", newName: "User_Id");
            RenameColumn(table: "dbo.Events", name: "TypeId", newName: "Type_Id");
            CreateIndex("dbo.Events", "User_Id");
            AddForeignKey("dbo.Events", "User_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
