using Domain.Abstraction.Model;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Abstraction.Repository
{
    public interface IProductLocationHistoryRepository
    {
        IEnumerable<IProductLocationInfo> All();
        IEnumerable<IProductLocationInfo> ByFilter(string filterColumn, string filter);
        Task<IEnumerable<IProductLocationInfo>> GetInfoByRegressionModelName(IRegressionLevel regressionLevel, string regressionModelName);
        Task<IEnumerable<IProductLocationInfo>> GetIncrementalInfoByRegressionModelName(IRegressionLevel regressionLevel, string regressionModelName);
        IEnumerable<IProductLocationInfo> ByFilter(IRegressionLevel regressionLevel, string filter);
    }
}
