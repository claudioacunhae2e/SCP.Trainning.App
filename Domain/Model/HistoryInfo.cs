using Domain.Abstraction.Model;
using System;

namespace Domain.Model
{
    public class HistoryInfo : IHistoryInfo
    {
        public double Price { get; }
        public DateTime Date { get; }
        public double InventoryMovements { get; }
        public double SalesRevenue { get; }
        public double SalesQuantity { get; }
        public double? QuantityOnHand { get; set; }
        public bool HasSales => SalesQuantity > 0;
        public bool HasInventory => QuantityOnHand > 0;


        public HistoryInfo(DateTime date, double inventoryMovements, double salesRevenue, double salesQuantity)
        {
            Date = date;
            InventoryMovements = inventoryMovements;
            SalesRevenue = salesRevenue;
            SalesQuantity = salesQuantity;
            Price = salesQuantity > 0
                        ? SalesRevenue / salesQuantity
                        : 0;
        }

        public HistoryInfo(DateTime date, double inventoryMovements, double quantityOnHand, double salesRevenue, double salesQuantity)
            : this(date, inventoryMovements, salesRevenue, salesQuantity)
        {
            QuantityOnHand = quantityOnHand;
        }
    }
}
