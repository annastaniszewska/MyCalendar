namespace MyCalendar.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class PopulateTypesTable : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Types (Id, Name) Values (1, 'Period')");
            Sql("INSERT INTO Types (Id, Name) Values (2, 'Ovulation')");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM Types WHERE Id IN (1, 2)");
        }
    }
}
