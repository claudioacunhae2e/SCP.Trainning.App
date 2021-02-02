using Domain.Abstraction.Model;
using E2E.SCP.RegressionModel.Abstraction.Model;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;

namespace Domain.Abstraction.Factory
{
    public interface ILoggerAsync
	{
		void Log(IModel model, IObservation observation, double weight, double error, double scalingFactor);
		void LogEnd(IModel model);
		void LogEnd(IRegressionModel model);
	}
}
