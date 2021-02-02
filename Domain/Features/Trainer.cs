using Domain.Abstraction.Factory;
using Domain.Abstraction.Model;
using Domain.Model;
using E2E.SCP.RegressionModel.Abstraction.Model;
using E2E.SCP.RegressionModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Features
{
    public class Trainer : IRefiner
	{
		public Trainer(IConfig configuration)
		{
			Configuration = configuration;
		}
		public IList<ILoggerAsync> Loggers { get; set; } = new List<ILoggerAsync>();
		public IConfig Configuration { get; }

		public IModel Train(IEnumerable<IObservation> observations, IModel prior)
		{

			double constant = CalculateConstant(observations, Configuration, prior);
			var priorModel = prior.Clone();
			priorModel.Coefficients[prior.ConstantID] = new Coefficient(constant, priorModel.Constant.Variation, priorModel.Constant.StandardError);

			var variationMatrix = CalculateVariationMatrix(Configuration.RootMeanSquaredError, priorModel);
			var result = priorModel;

			foreach (var observation in observations)
				(result, variationMatrix) = CalculateCoefficients(observation, variationMatrix, result, Configuration);

			LogEnd(result);

			return result;
		}

		private (IModel result, double[,] posteriorM) CalculateCoefficients(IObservation observation, double[,] variationMatrix, IModel prior, IConfig config)
		{
			var ajustedY = observation.Y + Config.AdjustToAvoidNegatives;
			var weight = CalculateWeight(ajustedY, config.UseWeightedRecursiveLeastSquares);
			var error = CalculateError(ajustedY, observation.Xs, prior);
			var (xTwB, bxxTwB) = CalculateXTwBAndBxxTwB(observation.Xs, variationMatrix, weight);
			var scalingFactor = CalculateScalingFactor(xTwB, observation.Xs, config.ForgettingFactor);
			var posteriorM = CalculatePosteriorInvertedCrossProductMatrix(variationMatrix, scalingFactor, bxxTwB, config.ForgettingFactor);
			var result = CalculateResults(scalingFactor, error, xTwB, posteriorM, config.RootMeanSquaredError, prior);
			LogResults(result, observation, weight, error, scalingFactor);

			return (result, posteriorM);
		}

		private void LogResults(IModel model, IObservation observation, double weight, double error, double scalingFactor)
		{
			if (Loggers.Any())
				foreach (var logger in Loggers)
					logger.Log(model, observation, weight, error, scalingFactor);
		}

		private void LogEnd(IModel model)
		{
			if (Loggers.Any())
				foreach (var logger in Loggers)
					logger.LogEnd(model);
		}

		private IModel CalculateResults(double scalingFactor, double error, double[] xTwB, double[,] priorErrorMatrix, double rootMeanSquaredError, IModel prior)
		{
			var length = xTwB.Count();
			var coefficients = new Dictionary<long, ICoefficient>();

			for (int i = 0; i < length; i++)
			{
				var coefficient = prior.Coefficients.ElementAt(i);
				coefficients.Add(coefficient.Key, CalculateCoefficient(coefficient.Value, scalingFactor, error, xTwB[i], priorErrorMatrix[i, i], rootMeanSquaredError));
			}

			return new Models(coefficients, prior.ConstantID);
		}

		private double CalculateConstant(IEnumerable<IObservation> observations, IConfig config, IModel prior)
		{
			var obs = observations.Take(config.DataPointsForPriorConstant).Select(o => Normalize(o, prior));
			return obs.Average();
		}
		private double Normalize(IObservation observation, IModel prior)
		{
			return Math.Log(observation.Y + Config.AdjustToAvoidNegatives) - prior.CalculateDemandEffects(observation.Xs);
		}

		private ICoefficient CalculateCoefficient(ICoefficient prior, double scalingFactor, double error, double xTwB, double variationBase, double rootMeanSquaredError)
		{
			var value = (scalingFactor * error * xTwB) + prior.Value;
			var variation = prior.Variation;
			var standartError = prior.StandardError;
			if (value != 0)
			{
				variation = (Math.Sqrt(variationBase) * rootMeanSquaredError) / value;
				standartError = value / (1 / variation);
			}
			return new Coefficient(value, variation, standartError);
		}
		private (double[] xTwB, double[,] BxxTwB) CalculateXTwBAndBxxTwB(IDictionary<long, double> features, double[,] variationMatrix, double weight)
		{
			var length = features.Count;
			var bxxTwB = new double[length, length];
			var xTwB = new double[length];
			var bx = new double[length];
			for (int col = 0; col < length; col++)
			{
				for (int row = 0; row < length; row++)
				{
					var feature = features.ElementAt(col);
					var invValue = variationMatrix[row, col];
					//Form xTwB: Multiply weighted new observation vector by prior inverted cross-product matrix
					xTwB[row] += feature.Value * weight * invValue;
					// Form Bx: Multiply prior inverted cross-product matrix by new observation vector
					bx[row] += feature.Value * invValue;
				}
			}


			// Form BxxTwB
			for (int row = 0; row < length; row++)
				for (int col = 0; col < length; col++)
					bxxTwB[row, col] = bx[row] * xTwB[col];

			return (xTwB, bxxTwB);
		}
		private double CalculateWeight(double ajustedObservation, bool useWeightedRecursiveLeastSquares) => useWeightedRecursiveLeastSquares ? Math.Sqrt(ajustedObservation / Math.Log(ajustedObservation)) : 1;
		private double CalculateError(double ajustedObservation, IDictionary<long, double> features, IModel prior) => Math.Log(ajustedObservation) - prior.CalculateDemandEffects(features);
		private double[,] CalculateVariationMatrix(double rootMeanSquaredError, IModel prior)
		{
			var length = prior.Coefficients.Count;
			var matrix = new double[length, length];
			for (int i = 0; i < length; i++)
			{
				var coeff = prior.Coefficients.ElementAt(i).Value;
				var standardError = coeff.Variation == 0 ? coeff.StandardError : coeff.Value * coeff.Variation;
				matrix[i, i] = Math.Pow(standardError / rootMeanSquaredError, 2);
			}

			return matrix;
		}
		private double CalculateScalingFactor(double[] xTwB, IDictionary<long, double> features, double forgettingFactor)
		{
			var dxTwBx = features.Zip(xTwB, (f, x) => f.Value * x).Sum();
			return 1 / (forgettingFactor + dxTwBx);
		}
		private double[,] CalculatePosteriorInvertedCrossProductMatrix(double[,] variationMatrix, double scalingFactor, double[,] bxxTwB, double forgettingFactor)
		{
			var length = variationMatrix.GetLength(0);
			var matrix = new double[length, length];
			for (int row = 0; row < length; row++)
				for (int col = 0; col < length; col++)
					matrix[row, col] = (1 / forgettingFactor) * (variationMatrix[row, col] - scalingFactor * bxxTwB[row, col]);

			return matrix;
		}
	}
}
