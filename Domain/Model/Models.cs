using E2E.SCP.RegressionModel.Abstraction.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Model
{
	public class Models : IModel
	{
		public virtual ICoefficient Constant { get; }
		public virtual long ConstantID { get; }
		public virtual bool TrainedWithLog { get; } = true;
		public virtual IDictionary<long, ICoefficient> Coefficients { get; }
		public Models(IDictionary<long, ICoefficient> coefficients, long constantID)
		{
			Coefficients = coefficients;
			ConstantID = constantID;
			Constant = constantID == 0 ? null : coefficients[constantID];
		}

		[JsonConstructor]
		public Models(ICoefficient constant, IDictionary<long, ICoefficient> coefficients, long constantID)
		{
			Constant = constant;
			Coefficients = coefficients;
			ConstantID = constantID;
		}

		public IModel Clone() => new Models(Coefficients.ToDictionary(c => c.Key, c => c.Value.Clone()), ConstantID);

		public double GetDemandEffects(IDictionary<long, double> featuresValuesXMatrix)
		{
			return TrainedWithLog ? Math.Exp(CalculateDemandEffects(featuresValuesXMatrix)) : CalculateDemandEffects(featuresValuesXMatrix);
		}
		public double CalculateDemandEffects(IDictionary<long, double> featuresValuesXMatrix)
		{
			return featuresValuesXMatrix.Select(x => x.Value * (Coefficients.ContainsKey(x.Key) ? Coefficients[x.Key].Value : 0)).Sum();
		}

		public double ForecastUsingLambda(IDictionary<long, double> featuresValuesXMatrix, double lambda) => GetDemandEffects(featuresValuesXMatrix) * lambda;
	}
}
