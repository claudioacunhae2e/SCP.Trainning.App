using System;

namespace Domain.Model
{
    public class ProductLocationHistoryDTO
	{
		public virtual long ID { get; set; }
		public virtual long Product { get; set; }
		public virtual long ProductLocation { get; set; }
		public virtual long Location { get; set; }
		public virtual DateTime Date { get; set; }
		public virtual double SalesQuantity { get; set; }
		public virtual double SalesRevenue { get; set; }
		public virtual double InventoryMovements { get; set; }
	}
}
