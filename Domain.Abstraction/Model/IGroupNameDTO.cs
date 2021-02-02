namespace Domain.Abstraction.Model
{
    public interface IGroupNameDTO : IParentName
	{
		int ProductLocationsCount { get; }
		int ProductLocationsCountQuartile { get; set; }
	}
}
