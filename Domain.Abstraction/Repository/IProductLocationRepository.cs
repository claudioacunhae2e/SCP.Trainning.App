using Domain.Abstraction.Model.Entitys;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using Old._42.SCP.Domain.Abstractions.Bases.Entitys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Abstraction.Repository
{
    public interface IProductLocationRepository : IReadOnlyRepository<IProductLocation>
    {
        IDictionary<long, IProductLocation> By(IRegressionLevel level, string key);
        Task<IDictionary<long, IProductLocation>> ByAsync(IRegressionLevel level, string key);
        IProductLocation By(long productID, long locationID);
        IDictionary<long, IProductLocation> BuildAndCacheByIDs(long[] productIds, long[] locationIds, long[] productLocations, object parameters = null);
        IDictionary<long, IProductLocation> ToSupplyAsDictionary(IEnumerable<long> ids);
    }
}
