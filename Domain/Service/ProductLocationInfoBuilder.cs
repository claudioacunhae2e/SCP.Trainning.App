using Domain.Abstraction.Model;
using Domain.Abstraction.Repository;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Service
{
    public class ProductLocationInfoBuilder : IProductLocationInfoBuilder
    {
        public ProductLocationInfoBuilder(IProductLocationHistoryRepository productLocationHistories)
        {
            ProductLocationHistories = productLocationHistories;
        }

        private readonly IProductLocationHistoryRepository ProductLocationHistories;

        public async Task<IEnumerable<IProductLocationInfo>> GetInfoByRegressionModelName(IRegressionLevel regressionLevel, string regressionModelName) =>
            await ProductLocationHistories.GetInfoByRegressionModelName(regressionLevel, regressionModelName);

        public async Task<IEnumerable<IProductLocationInfo>> GetIncrementalInfoByRegressionModelName(IRegressionLevel regressionLevel, string regressionModelName) =>
            await ProductLocationHistories.GetIncrementalInfoByRegressionModelName(regressionLevel, regressionModelName);
    }
}
