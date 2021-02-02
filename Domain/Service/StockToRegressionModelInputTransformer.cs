using Domain.Abstraction.Model;
using Domain.Model;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Service
{
    public class StockToRegressionModelInputTransformer : IStockToRegressionModelInputTransformer
	{
		protected IObservation GetDefaultObservations(IHistory history, IEnumerable<IFeature> features)
			=> new Observation(history.SalesRevenue, features.ToDictionary(f => f.ID, f => f.NotOcurenceValue));

		IInput GetInput(IEnumerable<IFeature> features, IHistory stock)
			=> new Input(stock.Date, GetDefaultObservations(stock, features));

		public IRegressionModelInput Transform<T>(IEnumerable<IFeature> features, IProductLocation<T> stock, string name, string parentName, long? itemID)
			where T : IHistory
		{
			return Transform(features, stock.Histories, name, parentName, itemID);
		}
		public IRegressionModelInput Transform<T>(IEnumerable<IFeature> features, IEnumerable<T> stocks, string name, string parentName, long? itemID)
			where T : IHistory
		{
			var input = new List<IInput>();
			foreach (var history in stocks.Where(s => s.QuantityOnHand > 0))
				input.Add(GetInput(features, history));
			return new RegressionModelInput(name, parentName, itemID, input);
		}
	}
}
