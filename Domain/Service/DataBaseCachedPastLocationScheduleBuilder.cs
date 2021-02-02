using Domain.Repository;
using Old._42.SCP.Domain.Abstractions.Bases.Entitys;

namespace Domain.Service
{
    public class DataBaseCachedPastLocationScheduleBuilder : DataBasePastLocationScheduleBuilder
    {
        public DataBaseCachedPastLocationScheduleBuilder(ILocationRepository locations, IDataBaseAccess dataBaseAccess)
            : base(locations, dataBaseAccess)
        {
        }
    }
}
