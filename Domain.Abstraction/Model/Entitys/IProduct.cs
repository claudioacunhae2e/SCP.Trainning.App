using Old._42.SCP.Domain.Abstractions.Bases.Entitys;
using System.Collections.Generic;

namespace Domain.Abstraction.Model.Entitys
{
    public interface IProduct : IEntity
    {
        string ClientID { get; set; }
        IDictionary<string, string> Attributes { get; }
        string GetGroupName(IEnumerable<string> groupers);
    }
}
