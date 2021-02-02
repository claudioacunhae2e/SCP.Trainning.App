using Domain.Abstraction.Factory;
using Domain.Abstraction.Model;

namespace Domain.Factory
{
    public class DateFeatureEngineerFactory : IFeatureEngineerFactory
    {
        public DateFeatureEngineerFactory(IInputFeatureEngineer dateFeatureEngineer)
        {
            _DateFeatureEngineer = dateFeatureEngineer;
        }

        private readonly IInputFeatureEngineer _DateFeatureEngineer;

        public IInputFeatureEngineer Get(ITimeScope timescope, long locationID) =>
            _DateFeatureEngineer;
    }
}
