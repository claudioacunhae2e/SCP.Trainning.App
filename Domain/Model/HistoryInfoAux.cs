using Domain.Abstraction.Model;

namespace Domain.Model
{
    public class HistoryInfoAux : IHistoryInfoAux
    {
        public HistoryInfoAux(
            bool hasSales,
            bool isValidBeta,
            bool hasStock,
            bool isStable,
            decimal alfa,
            decimal salesQuantity)
        {
            HasSales = hasSales;
            IsValidBeta = isValidBeta;
            HasStock = hasStock;
            IsStable = isStable;
            Alfa = alfa;
            SalesQuantity = salesQuantity;
        }

        public bool HasSales { get; }
        public bool IsValidBeta { get; }
        public bool HasStock { get; }
        public bool IsStable { get; }
        public decimal Alfa { get; }
        public decimal SalesQuantity { get; }
    }
}
