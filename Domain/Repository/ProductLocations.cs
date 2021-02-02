using Domain.Abstraction.Model;
using Domain.Abstraction.Model.Entitys;
using Domain.Abstraction.Repository;
using Domain.Model;
using Domain.Model.Entitys;
using Domain.Standard.Enums;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using E2E.SCP.RegressionModel.Extension;
using Old._42.SCP.Domain.Abstractions.Bases.Entitys;
using Old._42.SCP.Domain.Bases.Entitys;
using Old._42.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class ProductLocations : RepositoryBase<ProductLocationDTO, ProductLocationDTO>, IProductLocationRepository
    {
        private readonly IProductRepository _Products;
        private readonly ILocationRepository _Locations;

        public ProductLocations(IDataBaseAccess dataBaseAccess, IProductRepository products, ILocationRepository locations) : base(dataBaseAccess)
        {
            _Products = products;
            _Locations = locations;
        }

        private const string _QueryBase = @"
			   SELECT
					  ProductLocation.[ID]                          [ID],
					  ProductLocation.[Alfa]                        [Alfa],
					  ProductLocation.[Beta]                        [Beta],
					  ProductLocation.[Lambda]                      [Lambda],
					  ProductLocation.[StableBeta]                  [StableBeta],
					  ProductLocation.[StableAlfa]                  [StableAlfa],
					  ProductLocation.[QuantityOnHand]              [QuantityOnHand],
					  ProductLocation.[QuantityOnOrder]             [QuantityOnOrder],
					  ProductLocation.[QuantityReserved]            [QuantityReserved],
					  ProductLocation.[Product]                     [Product],
					  ProductLocation.[Location]                    [Location],
					  ProductLocation.[DefaultLambda]               [DefaultLambda],
					  ProductLocation.[DaysWithPositiveStock]       [DaysWithPositiveStock],
					  ProductLocation.[DaysWithSales]               [DaysWithSales],
					  ProductLocation.[TotalSales]                  [TotalSales],
					  ProductLocation.[StableDaysWithPositiveStock] [StableDaysWithPositiveStock],
					  ProductLocation.[StableDaysWithSales]         [StableDaysWithSales],
					  ProductLocation.[StableTotalSales]            [StableTotalSales],
					  ProductLocation.[FirstHistory]                [FirstHistory],
					  ProductLocation.[LastHistory]                 [LastHistory],
					  ProductLocation.[LambdaCalculationType]       [LambdaCalculationType]
				 FROM ProductLocation   ";

        private const string _BaseJoin = @" JOIN Product ON Product.ID = ProductLocation.Product
                                            JOIN Location ON Location.ID = ProductLocation.Location ";

        private IDictionary<long, IProductLocation> _ProductLocation { get; set; } = new Dictionary<long, IProductLocation>();

        public IList<IProductLocation> All() =>
            BuildAndCache().Select(d => d.Value).ToList();

        public IProductLocation ByID(long id) =>
            BuildAndCache()[id];

        public IList<IProductLocation> AllIgnored() =>
            All();

        public IProductLocation ByName(string name) =>
            All().FirstOrDefault(c => c.Name == name);

        public IList<IProductLocation> ByIDs(long[] ids) =>
            All().Where(i => ids.Contains(i.ID)).ToList();

        public IProductLocation Ignore(IProductLocation entity) =>
            ByID(entity.ID);

        public IDictionary<long, IProductLocation> AllAsDictionary() =>
            _ProductLocation;

        public IDictionary<long, IProductLocation> ToSupplyAsDictionary(IEnumerable<long> ids)
        {
            string baseQuery = BaseQuery("[ID] IN @part");

            var dtos = new List<ProductLocationDTO>();

            var arbitrarySize = 2000;
            foreach (var part in ids.BreakEach(arbitrarySize))
                dtos.AddRange(Query(baseQuery, new { part }));

            var products = _Products.ToSupplyAsDictionary(dtos.Select(x => x.Product).Distinct());
            var locations = _Locations.ToSupplyAsDictionary(dtos.Select(x => x.Location).Distinct());

            return Build(dtos, products, locations);
        }

        public IProductLocation By(long productID, long locationID) =>
            BuildAndCache().Values.FirstOrDefault(p => p.Location.ID == locationID && p.Product.ID == productID);

        private IDictionary<long, IProductLocation> BuildAndCache() =>
            BuildAndCache(_QueryBase);

        private IDictionary<long, IProductLocation> BuildAndCache(string baseQuery, object parameters = null)
        {
            return BuildAndCache(baseQuery, _Products.AllAsDictionary(), _Locations.AllAsDictionary(), parameters);
        }
        private IDictionary<long, IProductLocation> BuildAndCache(string baseQuery, IDictionary<long, IProduct> products, IDictionary<long, ILocation> locations, object parameters = null)
        {
            if (!_ProductLocation.Any())
            {
                _ProductLocation = Build(baseQuery, products, locations, parameters);

            }

            return _ProductLocation;
        }

        public IDictionary<long, IProductLocation> BuildAndCacheByIDs(long[] productIds, long[] locationIds, long[] productLocations, object parameters = null)
        {
            if (!_ProductLocation.Any())
            {
                var products = _Products.ByIDsAsDictionary(productIds);
                var locations = _Locations.ByIDsAsDictionary(locationIds);
                var where = " ProductLocation.[ID] IN @part";
                string baseQuery = BaseQuery(where);
                _ProductLocation = Build(baseQuery, products, locations, productLocations);
            }

            return _ProductLocation;
        }
        private IDictionary<long, IProductLocation> Build(string baseQuery, IDictionary<long, IProduct> products, IDictionary<long, ILocation> locations, object parameters = null)
        {
            var productLocationsDTO = Query(baseQuery, parameters);

            return Build(productLocationsDTO, products, locations);
        }
        private async Task<IDictionary<long, IProductLocation>> BuildAsync(string baseQuery, IDictionary<long, IProduct> products, IDictionary<long, ILocation> locations, object parameters = null)
        {
            var productLocationsDTO = await QueryAsync(baseQuery, parameters);

            return Build(productLocationsDTO, products, locations);
        }

        private IDictionary<long, IProductLocation> Build(string baseQuery, IDictionary<long, IProduct> products, IDictionary<long, ILocation> locations, long[] productLocations)
        {
            var productLocationsDTO = new List<ProductLocationDTO>();
            var arbitrarySize = 2000;
            foreach (var part in productLocations.BreakEach(arbitrarySize))
                productLocationsDTO.AddRange(Query(baseQuery, new { part }));

            return Build(productLocationsDTO, products, locations);
        }

        private IDictionary<long, IProductLocation> Build(IEnumerable<ProductLocationDTO> dtos, IDictionary<long, IProduct> products, IDictionary<long, ILocation> locations)
        {
            var productLocation = new Dictionary<long, IProductLocation>();

            foreach (var entity in dtos)
            {
                productLocation.Add(entity.ID, Build(products[entity.Product], locations[entity.Location], entity)); //TODO: Rafael alterei para poder fazer com que o cálculo incremental fique correto, mas tem que ser melhor avalido.
            }

            return productLocation;
        }

        private IProductLocation Build(IProduct product, ILocation location, ProductLocationDTO dto)
        {
            ProductLocationStats stats = null;
            if (dto.Lambda.HasValue)
            {
                stats = new ProductLocationStats(dto.Alfa.Value, dto.Beta.Value, dto.DaysWithPositiveStock.Value, dto.DaysWithSales.Value, dto.TotalSales.Value);
                stats.SetLambda(dto.Lambda.Value, (LambdaCalculationType)dto.LambdaCalculationType.Value);
            }
            else
            {
                stats = new ProductLocationStats(0, 0, 0, 0, 0);
            }

            var productLocation = new ProductLocation(
                        dto.ID,
                        dto.QuantityOnHand,
                        dto.QuantityOnOrder,
                        dto.QuantityReserved,
                        dto.DefaultLambda,
                        dto.EcomReserved,
                        product,
                        location,
                        stats,
                        dto.FirstHistory,
                        dto.LastHistory
                        );


            if (dto.StableBeta.IsNotNull())
            {
                var stableStats = new ProductLocationStats(dto.StableAlfa.Value, dto.StableBeta.Value, dto.StableDaysWithPositiveStock.Value, dto.StableDaysWithSales.Value, dto.StableTotalSales.Value);
                productLocation.SetStableStats(stableStats, 0, LambdaCalculationType.Invalid);
            }

            if (dto.StableBeta.IsNotNull())
            {
                var statsNew = new ProductLocationStats(dto.StableAlfa.Value, dto.StableBeta.Value, dto.StableDaysWithPositiveStock.Value, dto.StableDaysWithSales.Value, dto.StableTotalSales.Value);
                productLocation.SetStatsNew(statsNew, 0, LambdaCalculationType.Invalid);
            }

            return productLocation;
        }

        public IDictionary<long, IProductLocation> By(IRegressionLevel level, string key)
        {
            var locations = _Locations.AllAsDictionary(); // TODO: Rafael Não deveria ter aqui um Locations.BY para pegar as LocationsKeyGroupersText? 
            var products = _Products.By(level, key);
            var query = BaseQuery(level.AsWhere(key), _BaseJoin);
            return Build(query, products, locations);
        }

        public async Task<IDictionary<long, IProductLocation>> ByAsync(IRegressionLevel level, string key)
        {
            var locations = _Locations.AllAsDictionary();
            var products = await _Products.ByAsync(level, key);
            var query = BaseQuery(level.AsWhere(key), _BaseJoin);
            return await BuildAsync(query, products, locations);
        }

        private string BaseQuery(string where, string join = "")
        {
            var whereFormated = string.IsNullOrEmpty(where)
                                    ? string.Empty
                                    : string.Concat("\tWHERE ", where); //TODO: Rafael ProductLocation.[ID] = 502918 AND   remover este filtro usado para Debugar

            return string.Concat(_QueryBase, join, whereFormated);
        }

        #region Not Implemented
        public IDictionary<long, IProductLocation> ByIDsAsDictionary(long[] ids)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<IProductLocation>> IReadOnlyRepository<IProductLocation>.QueryAsync(string query, object parameters)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<IProductLocation>> IReadOnlyRepository<IProductLocation>.AllAsync(string scope = null)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
