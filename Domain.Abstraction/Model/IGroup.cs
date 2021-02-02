using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Abstraction.Model
{
	public interface IGroup : IProductLocation<IGroupHistory>
	{
		long RegressionLevel { get; }
		string Name { get; }
		string ParentName { get; }
		DateTime FirstSale { get; }
		DateTime LastSale { get; }
		int SalesAge { get; }
		double DaysWithSalesOverSalesAge { get; }
		double SalesQuantity { get; }
		double SalesRevenue { get; }
		int DaysWithSales { get; }
		int DaysWithPositiveStock { get; set; }
		bool IsMature(int minDaysWithSales, double minDaysWithSalesOverSalesAge);
	}
}
