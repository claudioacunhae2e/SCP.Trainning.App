using Domain.Standard.Enums;

namespace Domain.Abstraction.Model
{
    public interface ICanHaveMyLambdaReplacedBySimilar
    {
        long LocationID { get; }
        long ProductID { get; }
        IProductLocationStats Stats { get; }
        IProductLocationStats StableStats { get; }
        void SetStats(IProductLocationStats productLocationStats, decimal lambda, LambdaCalculationType lambdaCalculationType);
    }
}
