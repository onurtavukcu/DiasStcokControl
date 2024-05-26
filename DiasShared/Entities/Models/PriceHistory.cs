using DiasShared.Entities.BaseEntities;

namespace DiasShared.Entities.Models
{
    public class PriceHistory : BaseEntity
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int PriceTypeId { get; set; }
        public Product Product { get; set; }
        public PriceType PriceType { get; set; }
    }
}
