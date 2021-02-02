using Domain.Abstraction.Factory;
using Domain.Abstraction.Features;
using Domain.Abstraction.Model;
using Domain.Abstraction.Model.Entitys;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using E2E.SCP.RegressionModel.Standard.Enum;
using Old._42.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Domain.Factory
{
    [DataContract]
    [KnownType("GetKnownTypes")]
    public class DateFeatureEngineer :
        IInputFeatureEngineer,
        IDateFeatureIdentifierConfigured,
        IFeatureIdentifierFactoryStarted,
        IEventDateFeatureIdentifierFactoryLoadInput,
        IFeatureIdentifierFunctionLoader
    {
        private readonly IDictionary<DateTime, IDictionary<long, double>> _Dates = new Dictionary<DateTime, IDictionary<long, double>>();

        private IList<IFeature> Features { get; set; } = new List<IFeature>();

        private IDictionary<IFeature, IList<IEventDateFeature>> RegisteredFeatures { get; set; } = new Dictionary<IFeature, IList<IEventDateFeature>>();

        private IDictionary<IFeature, Func<DateTime, double>> Functions { get; set; } = new Dictionary<IFeature, Func<DateTime, double>>();

        private IDictionary<IFeature, Func<IEventDateFeature, DateTime, double>> InputedFunctions { get; set; } = new Dictionary<IFeature, Func<IEventDateFeature, DateTime, double>>();

        public void Transform(IEnumerable<IInput> data)
        {
            foreach (var input in data)
            {
                var values = _Dates[input.Date];

                foreach (var feature in values)
                {
                    if (input.Observation.Xs.ContainsKey(feature.Key))
                    {
                        input.Observation.Xs[feature.Key] = values[feature.Key];
                    }
                }
            }
        }

        public IInputFeatureEngineer Load(DateTime start, DateTime end)
        {
            var features = Features.Where(f => CanFindValues(f)).ToList();
            var daysUntil = start.ListDaysUntil(end);

            foreach (var day in daysUntil)
            {
                var featureDict = features.ToDictionary(f => f.ID, f => GetValueFor(f, day));
                _Dates.Add(day, featureDict);
            }

            return this;
        }

        private bool CanFindValues(IFeature feature) =>
            feature.Type == FeatureType.Date && (Functions.ContainsKey(feature) || RegisteredFeatures.ContainsKey(feature));

        private double CalculateInputedFeature(IList<IEventDateFeature> features, DateTime date, IFeature feature)
        {
            var ocurrenceFeature = features.FirstOrDefault(f => f.ItOccured(date));
            return ocurrenceFeature.IsNotNull()
                    ? CalculateInputedFeature(ocurrenceFeature, date)
                    : feature.NotOcurenceValue;
        }

        private double CalculateInputedFeature(IEventDateFeature feature, DateTime date)
        {
            return feature.Rule.IsNull()
                        ? feature.GetValue(date)
                        : InputedFunctions[feature.Feature](feature, date);
        }

        public IFeatureIdentifierFunctionLoader NextStep() =>
            this;

        public IFeatureIdentifierFunctionLoader AddCalculableFunctions<T>() where T : IDateFeatureFunctions, new()
        {
            var functions = new T().Functions;
            var features = Features.Where(f => f.Rule.IsNotNull() && functions.ContainsKey(f.Rule));

            foreach (var feature in features)
            {
                Functions.Add(feature, functions[feature.Rule]);
            }

            return this;
        }

        public IFeatureIdentifierFunctionLoader AddInputedFunctions<T>() where T : IInputedFeatureFunctions, new()
        {
            var functions = new T().Functions;
            var features = Features.Where(f => f.Rule.IsNotNull() && functions.ContainsKey(f.Rule));

            foreach (var feature in features)
            {
                InputedFunctions.Add(feature, functions[feature.Rule]);
            }

            return this;
        }

        public IEventDateFeatureIdentifierFactoryLoadInput Add(IList<IEventDateFeature> registeredFeatures)
        {
            foreach (var feature in registeredFeatures)
            {
                Add(feature);
            }

            return this;
        }

        IDateFeatureIdentifierConfigured IFeatureIdentifierFunctionLoader.NextStep() =>
            this;

        public IEventDateFeatureIdentifierFactoryLoadInput Add(IEventDateFeature registeredFeature)
        {
            if (RegisteredFeatures.ContainsKey(registeredFeature.Feature))
                RegisteredFeatures[registeredFeature.Feature].Add(registeredFeature);

            return this;
        }

        public IEventDateFeatureIdentifierFactoryLoadInput Add(params IEventDateFeature[] registeredFeatures) => Add(registeredFeatures.ToList());

        public IFeatureIdentifierFactoryStarted Add(IFeature feature)
        {
            Features.Add(feature);
            return this;
        }

        public IFeatureIdentifierFactoryStarted Add(IEnumerable<IFeature> features)
        {
            Features = Features.Concat(features).ToList();
            return this;
        }

        public IFeatureIdentifierFactoryStarted Add(params IFeature[] features) =>
            Add(features.ToList());

        public IEventDateFeatureIdentifierFactoryLoadInput Add(IEnumerable<IEventDateFeature> features) =>
            Add(features.ToList());

        IEventDateFeatureIdentifierFactoryLoadInput IFeatureIdentifierFactoryStarted.NextStep()
        {
            RegisteredFeatures = Features.ToDictionary(f => f, f => (IList<IEventDateFeature>)new List<IEventDateFeature>());
            return this;
        }

        public IDictionary<long, double> GetFeatures(DateTime date) =>
            _Dates[date];

        public IDictionary<DateTime, IDictionary<long, double>> GetFeatures(DateTime start, DateTime end) =>
            _Dates;

        private double GetValueFor(IFeature feature, DateTime date)
        {
            return Functions.ContainsKey(feature)
                    ? Functions[feature](date)
                    : CalculateInputedFeature(RegisteredFeatures[feature], date, feature);
        }
    }
}
