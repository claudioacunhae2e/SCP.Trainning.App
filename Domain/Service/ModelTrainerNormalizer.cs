using Domain.Abstraction.Factory;
using Domain.Abstraction.Model;
using Domain.Abstraction.Model.Entitys;
using Domain.Abstraction.Service;
using E2E.SCP.RegressionModel.Abstraction.Model;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Service
{
    public class ModelTrainerNormalizer : IModelTrainerNormalizer
    {
        public ModelTrainerNormalizer(
            IAlfaCalculatorConfig alfaNormalizerFactory,
            IProductLocationInfoBuilder productLocationInfoBuilder)
        {
            _AlfaNormalizerFactory = alfaNormalizerFactory;
            _ProductLocationInfoBuilder = productLocationInfoBuilder;
        }

        private readonly IAlfaCalculatorConfig _AlfaNormalizerFactory;
        private readonly IProductLocationInfoBuilder _ProductLocationInfoBuilder;

        public IEnumerable<IProductLocation> Calculate(
            IRegressionLevel level,
            string item,
            IModel model,
            IFeatureEngineerFactory factory,
            IList<IFeature> normalizationFeatures,
            ISystemInfo systemInfo)
        {
            var productLocations = _ProductLocationInfoBuilder.GetInfoByRegressionModelName(level, item).GetAwaiter().GetResult();
            var normalizer = _AlfaNormalizerFactory.Build(model, normalizationFeatures, systemInfo.Lambda.AlfaOutlier);

            foreach (var productLocation in productLocations)
            {
                productLocation.CalculateLambda(normalizer, factory, systemInfo.Lambda.MinBeta, systemInfo.HistoryStabilityLimitDateNewLambdaCalculation);
                yield return productLocation.ProductLocation;
            }
        }
    }
}
