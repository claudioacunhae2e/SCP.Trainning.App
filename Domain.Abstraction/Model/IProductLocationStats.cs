using Domain.Standard.Enums;

namespace Domain.Abstraction.Model
{
    public interface IProductLocationStats
    {
        LambdaCalculationType LambdaType { get; }
        decimal Lambda { get; }
        int Beta { get; }
        decimal Alfa { get; }
        int OpenStoreDaysWithPositiveStock { get; }
        int DaysWithSales { get; }
        decimal TotalSales { get; }
        bool IsValid();
        void SetLambda(decimal lambda, LambdaCalculationType lambdaType);
        void SetAlfaAndCalculateLambda(decimal alfa, int minBeta);
        void CalculateLambda(int minBeta);
        void AddHistoryStats(IHistoryInfoAux aux);
    }
}
