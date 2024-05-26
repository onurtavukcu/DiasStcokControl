using DiasAPI.ScheduledJobs;
using DiasDataAccessLayer.EntityDbContext;
using DiasDomain.ProductCommands.CRUDOperations;
using DiasShared.DTOs.InputDTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DiasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductDbContext _dbContext;

        private readonly IConfiguration _configuration;

        private readonly string delayedTime;

        public ProductController(ProductDbContext context, IConfiguration configuration)
        {
            _dbContext = context;
            _configuration = configuration;
            delayedTime = _configuration.GetSection("DelayTime").Value;
        }

        [HttpPost]
        public async Task<IActionResult> PostProductAsync(AddProductDTO product)
        {
            var command = new AddProductCommand(_dbContext);

            try
            {
                await command.AddProductCommandAsync(product);
                return Ok(product);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"{e.Message}");
            }
        }

        [HttpPut]
        [Route("DelayedPRoductUpdate")]
        public async Task<IActionResult> ProductUpdateAsync(UpdateProductDTO product)
        {
            bool canParse = int.TryParse(delayedTime, out int delayTime);

            if (!canParse)
            {
                throw new Exception("Delayed Time Must Be Integer");
            }

            try
            {
                var result = await UpdateProductjob.UpdateProduct(_dbContext, product, delayTime);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProduct([FromQuery] GetProductWithFilterDTO filters)
        {
            var command = new GetProductCommand(_dbContext);

            try
            {
                var products = await command.GetProductsCommandAsync(filters);

                var jsonSetting = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                };

                var result = JsonConvert.SerializeObject(products, Formatting.Indented, jsonSetting);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
