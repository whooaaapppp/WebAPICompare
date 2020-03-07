namespace WebApiNoTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelationupdatefromProducttoOrder : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "OrderPoco_Id", "dbo.Orders");
            DropIndex("dbo.Products", new[] { "OrderPoco_Id" });
            CreateTable(
                "dbo.OrderPocoProductPocoes",
                c => new
                    {
                        OrderPoco_Id = c.Guid(nullable: false),
                        ProductPoco_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.OrderPoco_Id, t.ProductPoco_Id })
                .ForeignKey("dbo.Orders", t => t.OrderPoco_Id, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductPoco_Id, cascadeDelete: true)
                .Index(t => t.OrderPoco_Id)
                .Index(t => t.ProductPoco_Id);
            
            DropColumn("dbo.Products", "OrderPoco_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "OrderPoco_Id", c => c.Guid());
            DropForeignKey("dbo.OrderPocoProductPocoes", "ProductPoco_Id", "dbo.Products");
            DropForeignKey("dbo.OrderPocoProductPocoes", "OrderPoco_Id", "dbo.Orders");
            DropIndex("dbo.OrderPocoProductPocoes", new[] { "ProductPoco_Id" });
            DropIndex("dbo.OrderPocoProductPocoes", new[] { "OrderPoco_Id" });
            DropTable("dbo.OrderPocoProductPocoes");
            CreateIndex("dbo.Products", "OrderPoco_Id");
            AddForeignKey("dbo.Products", "OrderPoco_Id", "dbo.Orders", "Id");
        }
    }
}
