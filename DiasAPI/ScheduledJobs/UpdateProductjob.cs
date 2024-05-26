using DiasDataAccessLayer.EntityDbContext;
using DiasDomain.ProductCommands.CRUDOperations;
using DiasShared.DTOs.InputDTO;
using Hangfire;

namespace DiasAPI.ScheduledJobs
{
    public static class UpdateProductjob
    {
        public async static Task<string> UpdateProduct(ProductDbContext dbContext, UpdateProductDTO product, int time)
        {
            var command = new UpdateProductCommand(dbContext);

            var hangfireJob =
                BackgroundJob
                .Schedule(
                    () => command.UpdateProductCommandAsync(product),
                    TimeSpan.FromSeconds(time));

            return hangfireJob;
        }
    }
}
