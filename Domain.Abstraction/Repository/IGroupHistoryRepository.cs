using Domain.Abstraction.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Abstraction.Repository
{
    public interface IGroupHistoryRepository
    {
        Task Save(params IGroup[] groups);
        IGroup By(long levelID, string name);
        Task<IEnumerable<IParentName>> GetGroupNamesToTrain(long levelID);
        Task UpdateGroupHistory();
        Task ClearTempTable();
    }
}
