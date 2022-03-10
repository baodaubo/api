using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly EntityDBContext db;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            db = new EntityDBContext();
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("{id}")]
        public ActionResult<WeatherForecast> GetTodoItem(long id)
        {
            if (id > Summaries.Length - 1)
            {
                return StatusCode(404);
            }

            string Item = Array.Find(Summaries, x => x == Summaries[id]);

            if (Item == null)
            {
                return StatusCode(404);
            }

            var rng = new Random();
            var todoItem = new WeatherForecast
            {
                Date = DateTime.Now.AddDays(id),
                TemperatureC = rng.Next(-20, 55),
                Summary = Item
            };

            return todoItem;
        }

        [HttpGet("Products")]
        public async Task<IEnumerable<Product>> GetProducts()
        {
            var products = db.Products.ToList();

            return products;
        }

        [HttpPost("AddNewProduct")]
        public async Task<ActionResult<Product>> AddProduct(Product p)
        {
            if (ModelState.IsValid)
            {
                await db.Products.AddAsync(p);
                await db.SaveChangesAsync();
            }
            else
                return StatusCode(500);

            return Ok("ADD SUCCESSED");
        }

        [HttpPut("EditProduct")]
        public async Task<ActionResult<Product>> EditProduct(Product p)
        {
            if (ModelState.IsValid)
            {
                var findp = db.Products.FirstOrDefault(x => x.productId == p.productId);

                if (findp == null)
                    return StatusCode(404);

                findp.productName = p.productName;
                db.Entry(findp).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else
                return StatusCode(500);

            return Ok("UPDATE SUCCESSED");
        }

        [HttpDelete("RemoveProduct/{id}")]
        public async Task<ActionResult<Product>> RemoveProduct(int id)
        {
            var findp = db.Products.FirstOrDefault(x => x.productId == id);

            if (findp == null)
                return StatusCode(404);

            db.Products.Remove(findp);
            await db.SaveChangesAsync();


            return Ok("DELETE SUCCESSED");
        }
    }
}
