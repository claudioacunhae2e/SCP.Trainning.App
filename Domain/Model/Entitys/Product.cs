using Domain.Abstraction.Model.Entitys;
using Newtonsoft.Json;
using Old._42.SCP.Domain.Bases.Entitys;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Model.Entitys
{
    public class Product : Entity, IProduct
    {
        public virtual string ClientID { get; set; }
        public virtual IDictionary<string, string> Attributes { get; set; }
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
        public virtual string GetGroupName(IEnumerable<string> groupers) => string.Join("|", groupers.Select(g => Attributes[g]));
        public Product()
        {
        }

        public Product(long id, string name, IDictionary<string, string> attributes)
        {
            ID = id;
            Attributes = attributes;
            Name = name;
        }
    }
}
