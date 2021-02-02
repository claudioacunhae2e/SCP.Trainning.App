using Domain.Abstraction.Model;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Service
{
    public interface IProductLocationInfoBuilder
    {
        Task<IEnumerable<IProductLocationInfo>> GetInfoByRegressionModelName(IRegressionLevel regressionLevel, string regressionModelName);
        Task<IEnumerable<IProductLocationInfo>> GetIncrementalInfoByRegressionModelName(IRegressionLevel regressionLevel, string regressionModelName);
    }
}
