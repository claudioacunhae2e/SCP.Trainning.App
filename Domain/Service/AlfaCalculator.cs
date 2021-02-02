using Domain.Abstraction.Factory;
using Domain.Abstraction.Service;
using E2E.SCP.RegressionModel.Abstraction.Model;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using Old._42.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Service
{
    public class AlfaCalculator : IAlfaCalculator
    {
        public AlfaCalculator(IModel model, IEnumerable<IFeature> features, double? alfaOutlier)
        {
            _Model = model;
            _AlfaOutlier = alfaOutlier;
            _Features = features.Distinct();
            _Normalize = model.IsNotNull();
        }

        private const double _ZedoD = 0d;

        private readonly bool _Normalize;
        private readonly IModel _Model;
        private readonly IEnumerable<IFeature> _Features;
        private readonly double? _AlfaOutlier;

        public double Normalize(double salesQuantity, DateTime date, IInputFeatureEngineer engineer)
        {
            return (_Normalize && _Model != null)
                        ? NormalizeExec(salesQuantity, date, engineer)
                        : _ZedoD;
        }

        private double NormalizeExec(double salesQuantity, DateTime date, IInputFeatureEngineer engineer)
        {
            var featureValues = GetFeaturesValues(date, engineer);
            var demandEffects = _Model.GetDemandEffects(featureValues);

            return Normalize(salesQuantity, demandEffects);
        }

        private Dictionary<long, double> GetFeaturesValues(DateTime date, IInputFeatureEngineer engineer)
        {
            var features = engineer.GetFeatures(date);
            var xs = new Dictionary<long, double>();

            foreach (var item in _Features)
            {
                if (features.TryGetValue(item.ID, out double value) && !xs.ContainsKey(item.ID))
                {
                    xs.Add(item.ID, value);
                }
            }

            return xs;
        }

        private double Normalize(double salesQuantity, double demandEffects)
        {
            var alfa = (demandEffects > 0)
                                    ? (salesQuantity / demandEffects)
                                    : _ZedoD;

            return alfa;
        }
    }
}
