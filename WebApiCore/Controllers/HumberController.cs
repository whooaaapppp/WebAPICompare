using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HumberController : ControllerBase
    {
        private readonly OrderContext _context;

        public HumberController()
        {
            _context = new OrderContext();
        }

        [HttpGet]
        [Route("get/{id}")]

        public ActionResult GetOrder(Guid id)
        {
            OrderPoco poco = _context.Orders
                .Include(o => o.Products)
                .ThenInclude(op => op.Product)
                .Where(o => o.Id == id)
                .FirstOrDefault();




            if (poco != null)
            {
                return Ok(poco);
            }
            else
            {
                return NotFound();
            }
        }
    }

    public class OrderContext : DbContext
    {
        public DbSet<ProductPoco> Products { get; set; }
        public DbSet<OrderPoco> Orders { get; set; }

        //public DbSet<OrderProductPoco> OrderProduct { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=WebApiNoTemplate.Controllers.OrderContext;Integrated Security=True;");
            //optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderProductPoco>()
                .HasKey(k => new { k.OrderPoco, k.ProductPoco });

            modelBuilder.Entity<OrderProductPoco>()
                .HasOne(op => op.Order)
                .WithMany(o => o.Products)
                .HasForeignKey(op => op.OrderPoco);

            modelBuilder.Entity<OrderProductPoco>()
                .HasOne(p => p.Product)
                .WithMany(op => op.Orders)
                .HasForeignKey(p => p.ProductPoco);


            modelBuilder.Entity<OrderPoco>()
                .HasKey(o => o.Id);

            modelBuilder.Entity<ProductPoco>()
                .HasKey(p => p.Id);

            base.OnModelCreating(modelBuilder);
        }
    }

    [Table("OrderProduct")]
    public class OrderProductPoco
    {
        [Column("OrderPoco_Id")]
        public Guid OrderPoco { get; set; }
        //[JsonIgnore]
        public virtual OrderPoco Order { get; set; }
        [Column("ProductPoco_Id")]
        public Guid ProductPoco { get; set; }
        //[JsonIgnore]
        public virtual ProductPoco Product { get; set; }
    }


    [Table("Products")]
    public class ProductPoco
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        //[JsonIgnore]
        public virtual ICollection<OrderProductPoco> Orders { get; set; }
    }

    [Table("Orders")]
    public class OrderPoco
    {
        public Guid Id { get; set; }
        public int OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public virtual ICollection<OrderProductPoco> Products { get; set; }
    }

}