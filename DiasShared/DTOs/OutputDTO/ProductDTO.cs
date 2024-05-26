namespace DiasShared.DTOs.OutputDTO
{
    public class ProductDTO
    {
        public string Name { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public List<PriceHistoryDTO> PriceHistories { get; set; }
    }
}
