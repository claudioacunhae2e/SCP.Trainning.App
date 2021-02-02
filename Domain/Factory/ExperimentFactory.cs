using Domain.Abstraction.Factory;
using Domain.Abstraction.Model;
using Domain.Features;
using E2E.SCP.RegressionModel.Abstraction.Model;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using E2E.SCP.RegressionModel.Model.Entitys;
using Old._42.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Factory
{
    public class ExperimentFactory<T> : ExperimentFactory, IFactoryInput<T>
    {
        IEnumerable<T> RawInputs { get; set; }
        IDataToInputTransformer<T> Transformer { get; set; }
        IRegressionLevel RegressionLevel { get; set; }
        public IFactoryInput<T> Input(IRegressionLevel regressionLevel, IEnumerable<T> inputs)
        {
            RawInputs = inputs;
            RegressionLevel = regressionLevel;
            return this;
        }

        public IFactoryInputFilter InputTransformer(IDataToInputTransformer<T> transformer)
        {
            return base.Input(RegressionLevel, transformer.Transform(RawInputs));
        }

        public IFactoryInputFilter InputTransformer<G>() where G : IDataToInputTransformer<T>, new()
        {
            Transformer = new G();
            return this;
        }

        public IFactoryModelInitializer ObservationTransformer(IDataToObservationTransformer<T> transformer)
        {
            throw new NotImplementedException();
        }

        public IFactoryModelInitializer ObservationTransformer<G>() where G : IDataToObservationTransformer<T>, new()
        {
            throw new NotImplementedException();
        }
    }

    public class ExperimentFactory :
        IExperimentFactory,
        IExperimentFactoryBuilder,
        IFactoryConfigured,
        IFactoryFeatureEngineer,
        IFactoryModelEvaluator,
        IFactoryModelInitializer,
        IFactoryModelRefiner,
        IFactoryFeaturesInput,
        IFactoryInputFilter
    {
        private IModel Prior { get; set; }
        private IList<IInputFeatureEngineer> InputFeatureEngineers { get; set; } = new List<IInputFeatureEngineer>();
        private IRefiner Refiner { get; set; }
        private IList<IInputFilter> Filters { get; set; } = new List<IInputFilter>();
        private IRegressionLevel RegressionLevel { get; set; }
        private IEnumerable<IRegressionModelInput> Inputs { get; set; }
        private IList<IFeature> Features { get; set; } = new List<IFeature>();

        public IFactoryFeatureEngineer AddFeatureEngineer(IInputFeatureEngineer featureEngineer)
        {
            if (featureEngineer.IsNotNull())
            {
                InputFeatureEngineers.Add(featureEngineer);
            }

            return this;
        }

        public IFactoryModelRefiner AddModelInitializer(IModel model)
        {
            Prior = model;
            return this;
        }

        public IFactoryModelEvaluator AddModelRefiner(IRefiner refiner)
        {
            Refiner = refiner;
            return this;
        }

        public IFactoryInputFilter Input(IRegressionLevel regressionLevel, IEnumerable<IRegressionModelInput> inputs)
        {
            RegressionLevel = regressionLevel;
            Inputs = inputs;
            Features = new List<IFeature>();

            return this;
        }

        public IFactoryInputFilter Input(IRegressionLevel regressionLevel, IRegressionModelInput inputs)
        {
            var inputsParam = new List<IRegressionModelInput>()
            {
                inputs
            };

            return Input(regressionLevel, inputsParam);
        }

        public IFactoryModelInitializer Next() =>
            this;

        public IEnumerable<IRegressionModel> Train()
        {
            var models = new List<IRegressionModel>();

            foreach (var input in Inputs)
            {
                if (input.Inputs.Any())
                {
                    var model = Train(input);

                    if (model.IsNotNull())
                    {
                        models.Add(model);
                    }
                }
            }

            return models;
        }

        public IRegressionModel Train(IRegressionModelInput input)
        {
            TrainFilters(input.Inputs);
            TrainFeatureEngineers(input);

            return input.Inputs.Any()
                        ? Train(input.Inputs, Prior, input.Name)
                        : null;
        }

        IFactoryConfigured IFactoryModelEvaluator.Next() =>
            this;

        public IFactoryFeaturesInput Add(IList<IFeature> features)
        {
            Features = Features.Concat(features).ToList();
            return this;
        }

        IFactoryFeatureEngineer IFactoryFeaturesInput.Next() =>
            this;

        public IFactoryInputFilter Add(IInputFilter filter)
        {
            Filters.Add(filter);
            return this;
        }

        IFactoryFeaturesInput IFactoryInputFilter.Next() =>
            this;

        public IFactoryConfigured Bayesian(IRegressionLevel level, IRegressionModelInput input, IModel prior, IInputFeatureEngineer[] engineers)
        {
            level.GetConfig();

            var trainer = new Trainer(level.Config.BayesianRegressor);
            var factory = BayesianBuildFactoryFeatureEngineer(level, input);

            BayesianAddFeatureEngineer(engineers, factory);

            return BayesianGetResult(prior, trainer, factory);
        }

        private IFactoryFeatureEngineer BayesianBuildFactoryFeatureEngineer(IRegressionLevel level, IRegressionModelInput input)
        {
            return new ExperimentFactory()
                                .Input(level, input)
                                .Add(new OutlierFilter(level.Config.OutliersConfig.Min, level.Config.OutliersConfig.Max))
                                .Next()
                                .Add(level.Features)
                                .Next();
        }

        private void BayesianAddFeatureEngineer(IInputFeatureEngineer[] engineers, IFactoryFeatureEngineer factory)
        {
            foreach (var engineer in engineers)
            {
                factory.AddFeatureEngineer(engineer);
            }
        }

        private IFactoryConfigured BayesianGetResult(IModel prior, Trainer trainer, IFactoryFeatureEngineer factory)
        {
            return factory
                        .Next()
                        .AddModelInitializer(prior)
                        .AddModelRefiner(trainer)
                        .Next();
        }

        private void TrainFeatureEngineers(IRegressionModelInput input)
        {
            foreach (var engineer in InputFeatureEngineers)
            {
                engineer.Transform(input.Inputs);
            }
        }

        private void TrainFilters(IList<IInput> inputs)
        {
            foreach (var filter in Filters)
            {
                filter.Filter(inputs);
            }
        }

        private IRegressionModel Train(IList<IInput> inputs, IModel prior, string name)
        {
            var observations = inputs.Select(i => i.Observation);
            var model = Refiner.Train(observations, prior);

            return new RegressionModel(name, model, RegressionLevel.ID, true);
        }
    }
}
