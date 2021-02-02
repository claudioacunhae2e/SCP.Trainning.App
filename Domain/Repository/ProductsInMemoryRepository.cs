using Domain.Abstraction.Model.Entitys;
using Domain.Abstraction.Repository;
using Domain.Model.Entitys;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using E2E.SCP.RegressionModel.Extension;
using Old._42.SCP.Domain.Abstractions.Bases.Entitys;
using Old._42.SCP.Domain.Bases.Entitys;
using Old._42.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class ProductsInMemoryRepository : BaseInMemoryRepository<IProduct>, IProductRepository
    {
        private const int _ArbitrarySize = 2000;

        public ProductsInMemoryRepository(IDataBaseAccess dataBaseAccess) : base("Product", dataBaseAccess)
        {
        }

        public IDictionary<long, IProduct> FindByIdAsDictionary(IEnumerable<long> ids)
        {
            var inClause = string.Join(",", ids);
            var query = string.Concat("SELECT * FROM PRODUCT WHERE ID IN ( ", inClause, " )");

            return ListAsDictionary(query);
        }

        public IDictionary<long, IProduct> By(IRegressionLevel level, string key)
        {
            var query = GetRegressionLevelQueryWithFilter(level, key);
            return ListAsDictionary(query);
        }

        public async Task<IDictionary<long, IProduct>> ByAsync(IRegressionLevel level, string key)
        {
            var query = GetRegressionLevelQueryWithFilter(level, key);
            return await ListAsDictionaryAsync(query);
        }

        public IDictionary<long, IProduct> ToSupplyAsDictionary(IEnumerable<long> ids)
        {
            var entities = new Dictionary<long, IProduct>();
            var each = ids.BreakEach(_ArbitrarySize);

            foreach (var part in each)
            {
                var queryIds = string.Join(",", part);
                var query = string.Concat("SELECT * FROM Product WHERE [ID] IN ( ", queryIds, " )");
                var result = ListAsDictionary(query);

                entities.AddRange(result);
            }

            return entities;
        }

        protected override IProduct BuildEntity(DataRow row, IEnumerable<DataColumn> columns)
        {
            var columnsFiltered = columns.Where(x => !x.ColumnName.Equals("ID", StringComparison.InvariantCultureIgnoreCase) &&
                                                     !x.ColumnName.Equals("DESCRIPTION", StringComparison.InvariantCultureIgnoreCase));

            var attributes = row.ToDictionary(columnsFiltered);
            var id = row.Get<long>("ID");
            var name = row.Get<string>("Description");

            return new Product(id, name, attributes);
        }

        private string GetRegressionLevelQueryWithFilter(IRegressionLevel level, string key)
        {
            var conditions = level.AsProductWhere(key);
            var where = string.IsNullOrEmpty(conditions)
                            ? string.Empty
                            : string.Concat("WHERE ", conditions);

            var query = string.Concat("SELECT * FROM Product ", where);

            return query;
        }
    }
}
