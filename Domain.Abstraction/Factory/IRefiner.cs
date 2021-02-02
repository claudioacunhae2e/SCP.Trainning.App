using Domain.Abstraction.Model;
using E2E.SCP.RegressionModel.Abstraction.Model;
using System.Collections.Generic;

namespace Domain.Abstraction.Factory
{
    public interface IRefiner
	{
		IModel Train(IEnumerable<IObservation> observations, IModel prior);
	}
}
