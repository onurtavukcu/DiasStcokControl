using DiasShared.Entities.BaseEntities;
namespace DiasShared.Entities.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public ICollection<PriceHistory> PriceHistories { get; set; }
    }
}
