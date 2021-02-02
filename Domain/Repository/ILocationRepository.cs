using Domain.Abstraction.Model.Entitys;
using Old._42.SCP.Domain.Abstractions.Bases.Entitys;
using System.Collections.Generic;

namespace Domain.Repository
{
    public interface ILocationRepository : IReadOnlyRepository<ILocation>
    {
        ILocation By(string clientID);
        IEnumerable<ILocation> By(IEnumerable<string> locations);
        IDictionary<long, ILocation> ToSupplyAsDictionary(IEnumerable<long> ids);
    }
}
