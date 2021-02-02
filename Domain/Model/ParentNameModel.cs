using Domain.Abstraction.Model;

namespace Domain.Model
{
    public class ParentNameModel : IParentName
    {
        public string Name { get; set; }
        public string ParentName { get; set; }
    }
}
