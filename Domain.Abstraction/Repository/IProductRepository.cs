using Domain.Abstraction.Model.Entitys;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using Old._42.SCP.Domain.Abstractions.Bases.Entitys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Abstraction.Repository
{
    public interface IProductRepository : IReadOnlyRepository<IProduct>
    {
        Task<IDictionary<long, IProduct>> ByAsync(IRegressionLevel level, string key);
        IDictionary<long, IProduct> By(IRegressionLevel level, string key);
        IDictionary<long, IProduct> ToSupplyAsDictionary(IEnumerable<long> ids);
        IDictionary<long, IProduct> FindByIdAsDictionary(IEnumerable<long> ids);
    }
}
