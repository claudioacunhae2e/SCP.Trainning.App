using Domain.Abstraction.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Model
{
    public class GroupHistory : IGroupHistory
    {
        public GroupHistory()
        {
        }

        public GroupHistory(DateTime date, double quantityOnHand, double revenue, double salesQuantity, double movements)
        {
            Date = date;
            InventoryMovements = movements;
            QuantityOnHand = quantityOnHand;
            SalesRevenue = revenue;
            SalesQuantity = salesQuantity;

            Price = salesQuantity > 0
                        ? revenue / salesQuantity
                        : 0;
        }

        private readonly object _LockAdd = new object();

        public double Price { get; }
        public DateTime Date { get; }
        public long RegressionLevel { get; set; }
        public string Name { get; set; }
        public string ParentName { get; set; }
        public double? QuantityOnHand { get; set; }
        public double InventoryMovements { get; private set; }
        public double SalesRevenue { get; private set; }
        public double SalesQuantity { get; private set; }
        public double DistinctStocks { get; private set; } = 0;
        public double OpenStoresWithStock { get; private set; }
        public double SoftPromotion { get; private set; } = 0;
        public double MediumPromotion { get; private set; } = 0;
        public double IntensePromotion { get; private set; } = 0;
        public double Observed { get; set; }
        private HashSet<long> OpenLocations { get; set; } = new HashSet<long>();

        public double AvgProductsWithStockPerOpentLocationsWithStock() =>
            OpenStoresWithStock != 0
                ? DistinctStocks / OpenStoresWithStock
                : 0;

        public double PercentageOfSoftPromotion() =>
            DistinctStocks != 0
                ? SoftPromotion / DistinctStocks
                : 0;

        public double PercentageOfMediumPromotion() =>
            DistinctStocks != 0
                ? MediumPromotion / DistinctStocks
                : 0;

        public double PercentageOfIntensePromotion() =>
            DistinctStocks != 0
                ? IntensePromotion / DistinctStocks
                : 0;

        public void Add<T>(IProductLocation<T> group, IHistoryInfo productLocationHistory, bool locationIsOpen) where T : IHistory
        {
            lock (_LockAdd)
            {
                InventoryMovements += productLocationHistory.InventoryMovements;
                SalesRevenue += productLocationHistory.SalesRevenue;
                SalesQuantity += productLocationHistory.SalesQuantity;
                QuantityOnHand += productLocationHistory.QuantityOnHand.Value;

                if (productLocationHistory.HasSales || (productLocationHistory.HasInventory && locationIsOpen))
                {
                    DistinctStocks++;

                    if (!OpenLocations.Any(x => x == group.LocationID))
                    {
                        OpenLocations.Add(group.LocationID);
                        OpenStoresWithStock++;
                    }
                }
            }
        }
    }
}
