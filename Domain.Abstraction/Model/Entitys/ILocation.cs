using Domain.Standard.Enums;
using Old._42.SCP.Domain.Abstractions.Bases.Entitys;
using System.Collections.Generic;

namespace Domain.Abstraction.Model.Entitys
{
    public interface ILocation : IEntity
    {
        string ClientID { get; }
        string CDClientID { get; }
        double AverageSales { get; set; }
        LocationType Type { get; set; }

        IDictionary<string, string> Attributes { get; }
        string GetGroupName(IEnumerable<string> groupers);
    }
}
