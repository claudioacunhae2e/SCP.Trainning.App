using Domain.Abstraction.Factory;
using Domain.Abstraction.Model;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Factory
{
    public class ProductLocationGroupFeatureEngineer : IInputFeatureEngineer
	{
		static int AjustToAvoidDistortions { get; } = 1;
		IDictionary<string, Func<IGroupHistory, double>> Functions { get; } = new Dictionary<string, Func<IGroupHistory, double>>
		{
			{"LogOfOpenStoresWithStock",        g => Math.Log(g.OpenStoresWithStock + AjustToAvoidDistortions) },
			{"OpenStoresWithStock",             g => g.OpenStoresWithStock },
			{"LogOfDistinctProductsInStock",    g => Math.Log(g.DistinctStocks + AjustToAvoidDistortions) },
			{"DistinctProductsInStock",         g => g.DistinctStocks },
			{"LogOfAvgDistinctProductsInStock", g => Math.Log(g.AvgProductsWithStockPerOpentLocationsWithStock() + AjustToAvoidDistortions) },
			{"AvgDistinctProductsInStock",      g => g.AvgProductsWithStockPerOpentLocationsWithStock() },
			{"PercentageOfSoftPromotion",       g => g.PercentageOfSoftPromotion() },
			{"PercentageOfMediumPromotion",     g => g.PercentageOfMediumPromotion() },
			{"PercentageOfIntensePromotion",    g => g.PercentageOfIntensePromotion() },
			{"LogOfSoftPromotion",              g => Math.Log(g.SoftPromotion + AjustToAvoidDistortions)},
			{"LogOfMediumPromotion",            g => Math.Log(g.MediumPromotion + AjustToAvoidDistortions)},
			{"LogOfIntensePromotion",           g => Math.Log(g.IntensePromotion + AjustToAvoidDistortions)},
		};

		IDictionary<DateTime, IDictionary<long, double>> Dates { get; }
		IEnumerable<IFeature> Features { get; }

		public ProductLocationGroupFeatureEngineer(IGroup group, IEnumerable<IFeature> features)
			: this(group.Histories, features)
		{
		}
		public ProductLocationGroupFeatureEngineer(IEnumerable<IGroupHistory> histories, IEnumerable<IFeature> features)
		{
			var keys = Functions.Select(d => d.Key);
			Features = features.Where(f => keys.Contains(f.Rule));
			IDictionary<DateTime, IGroupHistory> historyDictionary = new Dictionary<DateTime, IGroupHistory>();
			foreach (var item in histories)
			{
				if (!historyDictionary.ContainsKey(item.Date))
					historyDictionary.Add(item.Date, item);
			}
			Dates = BuildFeaturesByDate(historyDictionary);
		}

		private IDictionary<DateTime, IDictionary<long, double>> BuildFeaturesByDate(IDictionary<DateTime, IGroupHistory> values)
		{
			var dates = new Dictionary<DateTime, IDictionary<long, double>>();
			foreach (var value in values)
			{
				var features = new Dictionary<long, double>();
				var history = value.Value;
				foreach (var feature in Features)
				{
					if (!features.ContainsKey(feature.ID))
						features.Add(feature.ID, Functions[feature.Rule](history));

				}
				dates.Add(history.Date, features);
			}
			return dates;
		}

		public void Transform(IEnumerable<IInput> inputs)
		{
			foreach (var input in inputs)
			{
				if (Dates.ContainsKey(input.Date))
				{
					var values = Dates[input.Date];

					foreach (var feature in Features)
						input[feature] = values[feature.ID];
				}
			}
		}

		public IDictionary<long, double> GetFeatures(DateTime date) => Dates[date];

		public IDictionary<DateTime, IDictionary<long, double>> GetFeatures(DateTime start, DateTime end) => Dates;
		public IDictionary<DateTime, IDictionary<long, double>> GetFeatures() => Dates;
	}
}
