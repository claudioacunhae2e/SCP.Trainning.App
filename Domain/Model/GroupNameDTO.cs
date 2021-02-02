using Domain.Abstraction.Model;

namespace Domain.Model
{
    public class GroupNameDTO : IGroupNameDTO
    {
        public GroupNameDTO(string name, string parentName, int productLocations)
        {
            Name = name;
            ParentName = parentName;
            ProductLocationsCount = productLocations;
            ProductLocationsCountQuartile = 1;
        }

        public GroupNameDTO(IParentName parentName)
        {
            Name = parentName.Name;
            ParentName = parentName.ParentName;
        }

        public string Name { get; }
        public string ParentName { get; }
        public int ProductLocationsCount { get; }
        public int ProductLocationsCountQuartile { get; set; }
    }
}
