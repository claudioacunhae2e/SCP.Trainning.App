using Old._42.SCP.Domain.Bases.Entitys;
using System;

namespace Domain.Model.Entitys
{
    public class ProductLocationSummary : Entity
    {
        public string ParentName { get; set; }
        public DateTime Date { get; set; }
        public double SalesQuantity { get; set; }
        public double InventoryMovements { get; set; }
        public double SalesRevenue { get; set; }
        public double QuantityOnHand { get; set; }
        public int ItensAmount { get; set; }
    }
}
