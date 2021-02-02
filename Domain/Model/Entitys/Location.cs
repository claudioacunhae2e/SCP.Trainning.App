using Domain.Abstraction.Model.Entitys;
using Domain.Standard.Enums;
using Newtonsoft.Json;
using Old._42.SCP.Domain.Bases.Entitys;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Model.Entitys
{
    public class Location : Entity, ILocation
	{
		public virtual double AverageSales { get; set; }
		public virtual string ClientID { get; set; }
		public virtual string CDClientID { get; set; }
		public virtual LocationType Type { get; set; }
		public virtual IDictionary<string, string> Attributes { get; set; }
		public virtual string GetGroupName(IEnumerable<string> groupers) => string.Join("|", groupers.Select(g => Attributes[g]));
		[JsonIgnore]
		public virtual IDictionary<string, string> AttributesMap
		{
			get => Attributes;
			set
			{
				Attributes = new Dictionary<string, string>();
				foreach (var key in value.Keys)
				{
					Attributes.Add(key, value[key]?.ToString() ?? string.Empty);
				}
			}
		}

		public Location()
		{
		}
		public Location(long id, IDictionary<string, string> attributes)
		{
			ID = id;
			Attributes = attributes;
		}
		public Location(long id, IDictionary<string, string> attributes, string name) : this(id, attributes)
		{
			Name = name;
		}
		public Location(long id, IDictionary<string, string> attributes, string name, LocationType type, string cdClientID, string clientID) : this(id, attributes, name)
		{
			Type = type;
			CDClientID = cdClientID;
			ClientID = clientID;
		}
	}
}
