using Domain.Abstraction.Model;
using Domain.Model.Entitys;
using System.Collections.Generic;

namespace Domain.Factory
{
    public class SummarizeToSalesRevenueRegression : SummarizeToInputTransformer
	{
		public override IEnumerable<IRegressionModelInput> Transform(IEnumerable<ProductLocationSummary> productLocationSummaries)
		{
			var regressionModelInputs = new List<IRegressionModelInput>();

			foreach (var productLocationSummary in productLocationSummaries)
			{
				regressionModelInputs.Add(BuildInput(productLocationSummary, (ProductLocationSummary p) => p.SalesRevenue));
			}

			return regressionModelInputs;
		}
	}
}
