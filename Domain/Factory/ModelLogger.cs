using Domain.Abstraction.Factory;
using Domain.Abstraction.Model;
using E2E.SCP.RegressionModel.Abstraction.Model;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using System;
using System.Diagnostics;

namespace Domain.Factory
{
    public class ModelLogger : ILoggerAsync
	{
		public void Log(IModel model, IObservation observation, double weight, double error, double scalingFactor)
		{
			throw new NotImplementedException();
		}

		public void LogEnd(IModel model)
		{
			throw new NotImplementedException();
		}

		public void LogEnd(IRegressionModel regressionModel)
		{
			Console.WriteLine($@"{regressionModel.Name} Trained on level {regressionModel.RegressionLevel}");
			Debug.WriteLine($@"{regressionModel.Name} Trained on level {regressionModel.RegressionLevel}");
		}
	}
}
