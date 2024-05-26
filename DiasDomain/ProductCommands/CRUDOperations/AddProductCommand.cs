using DiasDataAccessLayer.EntityDbContext;
using DiasDomain.ProductCommands.ValidationOperations;
using DiasShared.DTOs.InputDTO;
using DiasShared.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DiasDomain.ProductCommands.CRUDOperations
{
    public class AddProductCommand
    {
        private readonly ProductDbContext _dbContext;

        public AddProductCommand(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddProductCommandAsync(AddProductDTO productDto)
        {
            if (productDto == null)
            {
                throw new ArgumentNullException(nameof(productDto), "ProductDTO cannot be null.");
            }

            if (ProductValidationCommand.Validate(productDto))
            {
                throw new ArgumentException("Validation Is Failiure. Product Name Can Not Contains Digit and can not null, Product Price and StockQuantity Bigger Than 0 (ZERO)");
            }

            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var productRecord =
                    _dbContext
                    .Products
                    .Where(n => n.Name == productDto.Name)
                .ToList();

                if (productRecord.Count != 0)
                {
                    throw new ArgumentException("The record you want to add exists in the database.");
                }

                var currentPriceType = await _dbContext.PriceTypes.FirstOrDefaultAsync(pt => pt.PriceTypes == PriceTypeEnums.Current);

                var product = new Product
                {
                    Name = productDto.Name,
                    StockQuantity = productDto.StockQuantity,
                    Price = productDto.Price
                };

                _dbContext.Products.Add(product);
                await _dbContext.SaveChangesAsync();


                var priceHistory = new PriceHistory
                {
                    ProductId = product.id,
                    ProductName = productDto.Name,
                    Price = product.Price,
                    PriceTypeId = currentPriceType.id
                };

                _dbContext.PricesHistory.Add(priceHistory);

                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}