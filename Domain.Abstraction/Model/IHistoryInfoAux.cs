namespace Domain.Abstraction.Model
{
    public interface IHistoryInfoAux
    {
        bool HasSales { get; }
        bool IsValidBeta { get; }
        bool HasStock { get; }
        bool IsStable { get; }
        decimal Alfa { get; }
        decimal SalesQuantity { get; }
    }
}
