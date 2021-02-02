using Domain.Abstraction.Factory;
using Domain.Abstraction.Model;
using Domain.Standard.Enums;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using Old._42.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Factory
{
    public class ProductLocationFeatureEngineer : IInputFeatureEngineer
	{
		IEnumerable<IFeature> Features { get; }
		IDictionary<DateTime, IDictionary<long, double>> FeatureValuesByDate { get; } = new Dictionary<DateTime, IDictionary<long, double>>();

		IDictionary<string, Func<IPromotion, double>> Functions { get; } = new Dictionary<string, Func<IPromotion, double>>
		{
			{ "IsSoftPromotion",     p => p.IsNotNull() && p.Intensity == PromotionIntensity.Soft ? 1d : 0d },
			{ "IsMediumPromotion",   p => p.IsNotNull() && p.Intensity == PromotionIntensity.Medium ? 1d : 0d },
			{ "IsIntensePromotion",  p => p.IsNotNull() && p.Intensity == PromotionIntensity.Intense ? 1d : 0d },
		};

		public ProductLocationFeatureEngineer(IEnumerable<IFeature> features, IDictionary<long, List<IPromotion>> promotions, ITimeScope timescope, long locationID, string promotionKey)
		{
			var keys = Functions.Select(d => d.Key);
			Features = features.Where(f => keys.Contains(f.Rule));

			var filteredPromotions = GetPromotions(promotions, locationID, promotionKey);

			FeatureValuesByDate = BuildFeaturesByDate(filteredPromotions, timescope);
		}
		private IEnumerable<IPromotion> GetPromotions(IDictionary<long, List<IPromotion>> Promotions, long locationID, string promotionKey)
		{
			if (Promotions.TryGetValue(locationID, out var promotions))
				return promotions.Where(p => p.Match(promotionKey));
			return null;
		}
		private IDictionary<DateTime, IDictionary<long, double>> BuildFeaturesByDate(IEnumerable<IPromotion> promotions, ITimeScope timescope)
		{
			var dates = new Dictionary<DateTime, IDictionary<long, double>>();
			foreach (var date in timescope.DaysForward())
			{
				var promotion = promotions?.FirstOrDefault(p => p.Match(date));
				var features = Features.ToDictionary(f => f.ID, f => Functions[f.Rule](promotion));
				dates.Add(date, features);
			}
			return dates;
		}
		public void Transform(IEnumerable<IInput> inputs)
		{
			foreach (var input in inputs)
				if (FeatureValuesByDate.TryGetValue(input.Date, out var featureValues))
					foreach (var feature in Features)
						input[feature.ID] = featureValues[feature.ID];
		}
		public IDictionary<long, double> GetFeatures(DateTime date) => FeatureValuesByDate[date];
		public IDictionary<DateTime, IDictionary<long, double>> GetFeatures(DateTime start, DateTime end)
		{
			return FeatureValuesByDate;
		}
	}
}
