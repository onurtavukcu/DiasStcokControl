using DiasDataAccessLayer.EntityDbContext;
using DiasShared.DTOs.InputDTO;
using DiasShared.DTOs.OutputDTO;
using Microsoft.EntityFrameworkCore;

namespace DiasDomain.ProductCommands.CRUDOperations
{
    public class GetProductCommand
    {
        private readonly ProductDbContext _dbContext;
        public GetProductCommand(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsCommandAsync(GetProductWithFilterDTO filters)
        {
            var products = await
                _dbContext
                .Products
                .Include(p => p.PriceHistories)
                .Where(p =>
                            (!filters.StockQuantitiy.HasValue || p.StockQuantity == filters.StockQuantitiy.Value) &&
                            (!filters.Price.HasValue || p.Price == filters.Price.Value))
                .ToListAsync();

            var productDTO = products
                .Select(p => new ProductDTO
                {
                    Name = p.Name,
                    StockQuantity = p.StockQuantity,
                    Price = p.Price,
                    PriceHistories =
                    p.PriceHistories
                        .Select(ph => new PriceHistoryDTO
                        {
                            Price = ph.Price,
                            PriceTypeId = PriceTypeResolver(ph.PriceTypeId)
                        }).ToList()
                });

            return productDTO;
        }

        public string PriceTypeResolver(int priceTypeId)
        {
            return priceTypeId switch
            {
                1 => "old",
                2 => "current",
                _ => throw new ArgumentException($"Invalid PriceTypeId: {priceTypeId}")
            };
        }
    }
}
