using Domain.Abstraction.Factory;
using Domain.Abstraction.Features;
using Domain.Abstraction.Model;
using Domain.Abstraction.Repository;
using Domain.Model;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using Old._42.SCP.Domain.Abstractions.Bases;
using System.Collections.Generic;

namespace Domain.Features
{
    public class FeatureEngineerBuilder : IFeatureEngineerBuilder
    {
        public FeatureEngineerBuilder(
            IFeatureIdentifierFactory factory,
            IEventDateFeatureRepository events,
            IDomainConfig domainConfig)
        {
            Factory = factory;
            Events = events;
            DomainConfig = domainConfig;
        }

        private readonly IFeatureIdentifierFactory Factory;
        private readonly IEventDateFeatureRepository Events;
        private readonly IDomainConfig DomainConfig;

        public IFeatureIdentifierFactoryStarted Start() =>
            Factory.Start();

        public IInputFeatureEngineer Default(IEnumerable<IFeature> features)
        {
            var date = DomainConfig.Today.AddYears(-3);
            var timeScope = new TimeScope(date, DomainConfig.Today);

            return Default(features, timeScope);
        }

        public IInputFeatureEngineer Default(IEnumerable<IFeature> features, ITimeScope timeScope, string scope = "")
        {
            var inputedFeatures = Events.ByComponetized(timeScope.Start, timeScope.End, scope).Result;

            return Factory
                    .Start()
                    .Add(features)//these gather all the Global and Brazil features to be used by the model
                    .NextStep()
                    .Add(inputedFeatures)//these repeat all the features that happened on the observations (i.e. matrix X)
                    .NextStep()
                    .AddCalculableFunctions<GlobalDateFeatureFunctions>()
                    .AddCalculableFunctions<BrazilianDateFeatureFunctions>()
                    .AddInputedFunctions<InputedDateFeatureFunctions>()
                    .NextStep()
                    .Load(timeScope.Start, timeScope.End);
        }
    }
}
