using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApiNoTemplate.Controllers
{
    [RoutePrefix("api/humber")]
    public class HumberController : ApiController
    {
        private readonly OrderContext _context;

        public HumberController()
        {
            _context = new OrderContext();
        }

        [HttpGet]
        [Route("get/{id}")]
        public IHttpActionResult GetOrder(Guid id)
        {
            OrderPoco poco = _context.Orders
                .Where(o => o.Id == id)
                .FirstOrDefault();

            return Ok(poco);
        }

        [HttpPost]
        [Route("post")]
        public IHttpActionResult PostOrder([FromBody] OrderPoco poco)
        {

            List<ProductPoco> existProducts = new List<ProductPoco>();

            foreach (ProductPoco product in poco.Products)
            {
                ProductPoco pPoco =
                    _context
                    .Products
                    .Find(product.Id);

                if (pPoco != null)
                {
                    existProducts.Add(pPoco);
                }
            }

            existProducts.ForEach(e => poco.Products.Add(e));

            _context.Orders.Add(poco);
            _context.SaveChanges();
            return Ok();
        }
    }

    public class OrderContext : DbContext
    {
        public DbSet<ProductPoco> Products { get; set; }
        public DbSet<OrderPoco> Orders { get; set; }

        public OrderContext() : base(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=WebApiNoTemplate.Controllers.OrderContext;Integrated Security=True;")
        {
            Database.Log = l => Debug.WriteLine(l);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<OrderPoco>()
                .HasMany(o => o.Products)
                .WithMany(p => p.Orders)
                .Map(op => op.ToTable("OrderProduct"));

            base.OnModelCreating(modelBuilder);
        }

    }



    [Table("Products")]
    public class ProductPoco
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public ICollection<OrderPoco> Orders { get; set; }
    }

    [Table("Orders")]
    public class OrderPoco
    {
        public Guid Id { get; set; }
        public int OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public virtual ICollection<ProductPoco> Products { get; set; }
    }


}
