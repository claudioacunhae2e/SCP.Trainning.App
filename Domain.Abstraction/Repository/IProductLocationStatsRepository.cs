using Domain.Abstraction.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Abstraction.Repository
{
    public interface IProductLocationStatsRepository
    {
        Task Insert(IEnumerable<ProductLocationWithStatsDTO> productLocations);
        Task UpdateProductLocationStats();
        Task ClearTempTable();
    }
}
