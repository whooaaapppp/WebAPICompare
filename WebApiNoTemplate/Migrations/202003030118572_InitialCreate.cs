namespace WebApiNoTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OrderNo = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OrderPoco_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderPoco_Id)
                .Index(t => t.OrderPoco_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "OrderPoco_Id", "dbo.Orders");
            DropIndex("dbo.Products", new[] { "OrderPoco_Id" });
            DropTable("dbo.Products");
            DropTable("dbo.Orders");
        }
    }
}
