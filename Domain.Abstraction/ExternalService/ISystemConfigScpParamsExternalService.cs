using Domain.Abstraction.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Abstraction.ExternalService
{
    public interface ISystemConfigScpParamsExternalService
    {
        Task<List<string>> ItemsDistribute();
        Task<ISystemConfigDTO> Get();
        Task<IEnumerable<IProductLocationSupplyConfigDTO>> DistributionConfig(string regressionModelName, string cdClientId);
        Task<List<dynamic>> MirrorLocation(string enable);
    }
}
