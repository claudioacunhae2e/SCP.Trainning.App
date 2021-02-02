using System;

namespace Domain.Abstraction.Model
{
    public interface IHistory
    {
        DateTime Date { get; }
        double InventoryMovements { get; }
        double SalesRevenue { get; }
        double SalesQuantity { get; }
        double Price { get; }
        double? QuantityOnHand { get; set; }
    }
}
