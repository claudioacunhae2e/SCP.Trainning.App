using Domain.Abstraction.Model;

namespace Domain.Model
{
    public struct GroupHistoryDTO
    {
        public GroupHistoryDTO(IGroup group, IGroupHistory item)
        {
            RegressionLevel = group.RegressionLevel;
            Name = group.Name;
            Date = item.Date.ToString("yyyy-MM-dd");
            DistinctStocks = (decimal)item.DistinctStocks;
            IntensePromotion = (decimal)item.IntensePromotion;
            InventoryMovements = (decimal)item.InventoryMovements;
            MediumPromotion = (decimal)item.MediumPromotion;
            OpenStoresWithStock = (decimal)item.OpenStoresWithStock;
            ParentName = group.ParentName;
            Price = (decimal)item.Price;
            Observed = false;
            QuantityOnHand = (decimal)item.QuantityOnHand;
            SalesQuantity = (decimal)item.SalesQuantity;
            SalesRevenue = (decimal)item.SalesRevenue;
            SoftPromotion = (decimal)item.SoftPromotion;
        }

        public long RegressionLevel { get; set; }
        public string Name { get; set; }
        public string ParentName { get; set; }
        public string Date { get; set; }
        public decimal InventoryMovements { get; set; }
        public decimal SalesRevenue { get; set; }
        public decimal SalesQuantity { get; set; }
        public decimal QuantityOnHand { get; set; }
        public decimal Price { get; set; }
        public decimal DistinctStocks { get; set; }
        public decimal OpenStoresWithStock { get; set; }
        public bool Observed { get; set; }
        public decimal SoftPromotion { get; set; }
        public decimal MediumPromotion { get; set; }
        public decimal IntensePromotion { get; set; }
    }
}
