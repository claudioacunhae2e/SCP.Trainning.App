namespace Domain.Abstraction.Model
{
    public interface ISystemInfoLambda
    {
        int MinBeta { get; set; }
        int RectificationMinDataPoints { get; set; }
        int MinDataPointsToUseLocationVariationConstraints { get; set; }
        double MaxLocationVariation { get; set; }
        double MinLocationVariation { get; set; }
        double RectificationPercentil { get; set; }
        double Default { get; set; }
        long RectificationLevelID { get; set; }

        double? AlfaOutlier { get; set; }
    }
}
