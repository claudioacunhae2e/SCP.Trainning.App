using Domain.Abstraction.Model;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using System.Collections.Generic;

namespace Domain.Service
{
    public interface IStockToRegressionModelInputTransformer
	{
		IRegressionModelInput Transform<T>(IEnumerable<IFeature> features, IProductLocation<T> stock, string name, string parentName, long? itemID) where T : IHistory;
	}
}
