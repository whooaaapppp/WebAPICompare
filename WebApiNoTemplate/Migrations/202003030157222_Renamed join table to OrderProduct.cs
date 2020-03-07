namespace WebApiNoTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamedjointabletoOrderProduct : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.OrderPocoProductPocoes", newName: "OrderProduct");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.OrderProduct", newName: "OrderPocoProductPocoes");
        }
    }
}
