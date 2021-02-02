using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using Old._42.SCP.Domain.Abstractions.Bases.Entitys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Abstraction.Repository
{
    public interface IFeatureRepository : IReadOnlyRepository<IFeature>
    {
        IList<IFeature> ByRegressionLevel(long id);
        Task<IEnumerable<IFeature>> ByRegressionLevelAsync(long id, string scope = null);
        Task<IFeature> ByIDAsync(long id, string scope = null);
    }
}
