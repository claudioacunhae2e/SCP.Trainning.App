using Domain.Abstraction.Model;

namespace Domain.Model
{
    public class SystemInfoLambda : ISystemInfoLambda
    {
        public int MinBeta { get; set; }
        public int RectificationMinDataPoints { get; set; }
        public double RectificationPercentil { get; set; }
        public long RectificationLevelID { get; set; }
        public double Default { get; set; }
        public int MinDataPointsToUseLocationVariationConstraints { get; set; }
        public double MaxLocationVariation { get; set; }
        public double MinLocationVariation { get; set; }
        public virtual double? AlfaOutlier { get; set; }
    }
}
