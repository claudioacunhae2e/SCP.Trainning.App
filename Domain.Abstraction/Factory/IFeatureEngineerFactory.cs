using Domain.Abstraction.Model;

namespace Domain.Abstraction.Factory
{
    public interface IFeatureEngineerFactory
    {
        IInputFeatureEngineer Get(ITimeScope timescope, long locationID);
    }
}
