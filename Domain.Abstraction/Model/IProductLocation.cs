using System;
using System.Collections.Generic;

namespace Domain.Abstraction.Model
{
    public interface IProductLocation<T> where T : IHistory
	{
		long ProductID { get; }
		long LocationID { get; }
		double QuantityOnHand { get; }
		double QuantityOnOrder { get; }
		double QuantityReserved { get; }
		string PromotionKey { get; }
		IList<T> Histories { get; set; }
		T AddHistory(DateTime date, double quantityOnHand, double revenue, double salesQuantity, double movements);
	}
}
