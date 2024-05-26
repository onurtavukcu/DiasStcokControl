using DiasShared.Entities.BaseEntities;

namespace DiasShared.Entities.Models
{
    public class PriceType : BaseEntity
    {
        public PriceTypeEnums PriceTypes { get; set; }
        public ICollection<PriceHistory> PriceHistories { get; set; }
    }
}
