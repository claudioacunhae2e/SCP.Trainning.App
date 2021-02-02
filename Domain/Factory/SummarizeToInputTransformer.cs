using Domain.Abstraction.Factory;
using Domain.Abstraction.Model;
using Domain.Model;
using Domain.Model.Entitys;
using System;
using System.Collections.Generic;

namespace Domain.Factory
{
    public abstract class SummarizeToInputTransformer : IDataToInputTransformer<ProductLocationSummary>
	{
		public abstract IEnumerable<IRegressionModelInput> Transform(IEnumerable<ProductLocationSummary> productLocationSummaries);

		protected IRegressionModelInput BuildInput(ProductLocationSummary productLocationSummary, Func<ProductLocationSummary, double> accessor)
		{
			return
				new RegressionModelInput(
						productLocationSummary.Name,
						productLocationSummary.ParentName,
						null,
						new List<IInput> {
							new Input(productLocationSummary.Date, new Observation(accessor(productLocationSummary), new Dictionary<long, double>()))
						}
					);
		}
	}
}
