using DiasDataAccessLayer.EntityDbContext;
using DiasDomain.ProductCommands.ValidationOperations;
using DiasShared.DTOs.InputDTO;
using DiasShared.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DiasDomain.ProductCommands.CRUDOperations
{
    public class UpdateProductCommand
    {
        private readonly ProductDbContext _dbContext;
        public UpdateProductCommand(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task UpdateProductCommandAsync(UpdateProductDTO products)
        {
            if (products is null)
            {
                throw new ArgumentNullException(nameof(products), "ProductDTO cannot be null.");
            }

            if (!ProductValidationCommand.IntTypeValidator(products.ProductID) || !ProductValidationCommand.DoubleTypeValidator(products.Price))
            {
                throw new ArgumentException("Validation Is Failiure. Product Id And Product Price Must Be Numeric andBigger Than 0 (ZERO).");
            }

            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var productId = products.ProductID;

                var currentProduct = await _dbContext.Products.FindAsync(productId);

                if (currentProduct is null)
                {
                    throw new Exception("Product Not Found");
                }

                if (currentProduct.Price == products.Price)
                {
                    throw new Exception("The price value you entered is the same as current price");
                }

                currentProduct.Price = products.Price;

                _dbContext.Products.Update(currentProduct);
                await _dbContext.SaveChangesAsync();

                var currentPriceType = await _dbContext.PriceTypes.FirstOrDefaultAsync(pt => pt.PriceTypes == PriceTypeEnums.Current);
                var oldPriceType = await _dbContext.PriceTypes.FirstOrDefaultAsync(pt => pt.PriceTypes == PriceTypeEnums.Old);

                var oldPriceRecord = _dbContext
                    .PricesHistory
                    .Where(p => p.ProductId == productId)
                    .OrderByDescending(p => p.id)
                    .FirstOrDefaultAsync();

                if (oldPriceRecord != null)
                {
                    oldPriceRecord.Result.PriceTypeId = oldPriceType.id;
                    _dbContext.PricesHistory.Update(oldPriceRecord.Result);
                }

                var newPriceRecord = new PriceHistory
                {
                    ProductId = currentProduct.id,
                    Price = products.Price,
                    PriceTypeId = currentPriceType.id,
                    ProductName = currentProduct.Name
                };

                _dbContext.PricesHistory.Add(newPriceRecord);

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
