using Domain.Abstraction.Model;
using Domain.Abstraction.Model.Entitys;
using Domain.Abstraction.Repository;
using Domain.Model;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using E2E.SCP.RegressionModel.Extension;
using Old._42.SCP.Domain.Abstractions.Bases.Entitys;
using Old._42.SCP.Domain.Bases.Entitys;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class ProductLocationHistories : RepositoryBase<ProductLocationHistoryDTO, ProductLocationHistoryDTO>, IProductLocationHistoryRepository
    {
        public ProductLocationHistories(
            IDataBaseAccess dataBaseAccess,
            IProductLocationRepository productLocations,
            ISystemInfoRepository systemInfos) : base(dataBaseAccess)
        {
            _ProductLocations = productLocations;
            _SystemInfo = systemInfos.Get()
                                     .GetAwaiter()
                                     .GetResult();
        }

        private const string _QueryAnd = "  AND ";
        private const string _QueryBase = @"
            SELECT		
            		ProductLocationHistory.ProductLocation                   [ProductLocation],
            		ProductLocationHistory.Location                          [Location],
            		ProductLocationHistory.[Date]                            [Date],
            		ProductLocationHistory.[SalesQuantity]                   [SalesQuantity],
            		ProductLocationHistory.[SalesRevenue]                    [SalesRevenue],
            		ProductLocationHistory.[InventoryMovements]              [InventoryMovements]
            FROM	Product 
            JOIN	ProductLocationHistory   ON Product.ID = ProductLocationHistory.Product
            JOIN	Location  ON Location.ID = ProductLocationHistory.Location
            WHERE	Location.Type = 2  ";

        private readonly IProductLocationRepository _ProductLocations;
        private readonly ISystemInfo _SystemInfo;

        public IEnumerable<IProductLocationInfo> All() =>
            BuildInfo(Query(_QueryBase));

        public IEnumerable<IProductLocationInfo> ByFilter(IRegressionLevel regressionLevel, string filter) =>
            ByFilter(regressionLevel.AsQueryFilter().pattern, filter);

        public async Task<IEnumerable<IProductLocationInfo>> GetInfoByRegressionModelName(IRegressionLevel regressionLevel, string regressionModelName) =>
            await GetProductLocationAndHistoryInfoByRegressionModelName(regressionLevel, regressionModelName, string.Empty);

        public async Task<IEnumerable<IProductLocationInfo>> GetIncrementalInfoByRegressionModelName(IRegressionLevel regressionLevel, string regressionModelName)
        {
            var param = new
            {
                _SystemInfo.HistoryStabilityLimitDateLastLambdaCalculation
            };

            return await GetProductLocationAndHistoryInfoByRegressionModelName(regressionLevel, regressionModelName, "ProductLocationHistory.Date > @HistoryStabilityLimitDateLastLambdaCalculation", param);
        }

        async Task<IEnumerable<IProductLocationInfo>> GetProductLocationAndHistoryInfoByRegressionModelName(IRegressionLevel regressionLevel, string regressionModelName, string filter, object parameters = null)
        {
            var where = string.IsNullOrEmpty(filter)
                            ? regressionLevel.AsWhere(regressionModelName)
                            : string.Concat(regressionLevel.AsWhere(regressionModelName), _QueryAnd, filter);

            var query = BuildQueryBase(where);

            var productLocations = _ProductLocations.By(regressionLevel, regressionModelName);
            var productLocationHistories = await QueryAsync(query, parameters);

            return BuildInfo(productLocationHistories, productLocations);
        }

        public IEnumerable<IProductLocationInfo> ByFilter(string filterColumn, string filter)
        {
            var filterQuery = string.Concat(filterColumn, " = @filter");
            var query = BuildQueryBase(filterQuery);
            var param = new
            {
                filter
            };

            return BuildInfo(Query(query, param));
        }

        private IEnumerable<IProductLocationInfo> BuildInfo(IEnumerable<ProductLocationHistoryDTO> entities) =>
            BuildInfo(entities, _ProductLocations.AllAsDictionary());

        private IEnumerable<IProductLocationInfo> BuildInfo(IEnumerable<ProductLocationHistoryDTO> histories, IDictionary<long, IProductLocation> productLocations)
        {
            var productLocationDict = productLocations.ToDictionary(h => h.Key, h => new ProductLocationInfo(h.Value, _SystemInfo.PromotionKeyAttribute));

            foreach (var history in histories)
            {
                productLocationDict[history.ProductLocation].AddHistory(history.Date, history.InventoryMovements, history.SalesRevenue, history.SalesQuantity);
            }

            return productLocationDict.Select(h => h.Value);
        }

        private string BuildQueryBase(params string[] conditions)
        {
            var condtionsClean = conditions.Where(x => !string.IsNullOrWhiteSpace(x));
            var resultcondtionsClean = (condtionsClean != null && condtionsClean.Any())
                                            ? string.Join(_QueryAnd, condtionsClean)
                                            : string.Empty;

            var query = resultcondtionsClean.Length > 0
                            ? string.Concat(_QueryBase, _QueryAnd, resultcondtionsClean)
                            : _QueryBase;

            return query;
        }
    }
}
