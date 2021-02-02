using Domain.Abstraction.Factory;
using System;

namespace Domain.Model
{
    public class ProductLocationInfoCalculateLambda
    {
        public ProductLocationInfoCalculateLambda(IInputFeatureEngineer engineer, DateTime stability, int minBeta)
        {
            Engineer = engineer;
            Engineer = engineer;
            Stability = stability;
            MinBeta = minBeta;
            Alfa = 0d;
            StableAlfa = 0d;
        }

        public readonly IInputFeatureEngineer Engineer;
        public readonly DateTime Stability;
        public readonly int MinBeta;

        public double Alfa { get; set; }
        public double StableAlfa { get; set; }
    }
}
