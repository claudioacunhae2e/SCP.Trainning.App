namespace Domain.Abstraction.Model
{
    public interface IGroupHistory : IHistory
    {
        double DistinctStocks { get; }
        double OpenStoresWithStock { get; }
        double SoftPromotion { get; }
        double MediumPromotion { get; }
        double IntensePromotion { get; }
        double Observed { get; }
        double AvgProductsWithStockPerOpentLocationsWithStock();
        double PercentageOfSoftPromotion();
        double PercentageOfMediumPromotion();
        double PercentageOfIntensePromotion();
    }
}
