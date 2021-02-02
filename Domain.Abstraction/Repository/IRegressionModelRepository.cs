using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using Old._42.SCP.Domain.Abstractions.Bases.Entitys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Abstraction.Repository
{
    public interface IRegressionModelRepository : IRepository<IRegressionModel>
    {
        IEnumerable<IRegressionModel> ByLevel(long levelID);
        Task<IEnumerable<IRegressionModel>> ByLevelAsync(long levelID, string scope = null);
        Task<IEnumerable<IRegressionModel>> ByLevelAsync(long levelID, IEnumerable<string> keys);
        void Add(IRegressionModel regressionModel);
        void Add(IEnumerable<IRegressionModel> regressionModels);
        Task Update(IRegressionModel model);
    }
}
