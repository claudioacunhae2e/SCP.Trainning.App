using Domain.Abstraction.ExternalService;
using Domain.Abstraction.Model;
using Domain.Model;
using E2E.Infra.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ExternalService
{
    public class SystemConfigScpParamsExternalService : ISystemConfigScpParamsExternalService
    {
        public SystemConfigScpParamsExternalService(HttpClient client)
        {
            _Client = new HttpRequest(client);
        }

        private readonly HttpRequest _Client;

        public async Task<ISystemConfigDTO> Get()
        {
            var request = await _Client.Get<SystemConfigDTO>("/api/system-config");
            return request.Content;
        }
        public async Task<IEnumerable<IProductLocationSupplyConfigDTO>> DistributionConfig(string regressionModelName, string cdClientId)
        {
            var request = await _Client.Get<List<ProductLocationSupplyConfigDTO>>($"/api/supply-configs/distribution-configs/{regressionModelName}/DistributionCenters/{cdClientId}");
            return request.Content;
        }
        public async Task<List<string>> ItemsDistribute()
        {
            var request = await _Client.Get<List<string>>("/api/supply-configs/items-to-distribute");
            return request.Content;
        }
        public async Task<List<dynamic>> MirrorLocation(string enable)
        {
            enable = !string.IsNullOrEmpty(enable) ? string.Concat("?Enable=", enable) : "";
            var request = await _Client.Get<List<dynamic>>(string.Concat("/api/mirror-location", enable), true);
            return request.Content;
        }
    }
}
