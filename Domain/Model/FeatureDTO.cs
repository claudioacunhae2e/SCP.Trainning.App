using E2E.SCP.RegressionModel.Standard.Enum;

namespace Domain.Model
{
    public class FeatureDTO
	{
		public virtual long ID { get; set; }
		public virtual string Name { get; set; }
		public virtual string Rule { get; set; }
		public virtual double Coefficient { get; set; }
		public virtual double Variation { get; set; }
		public virtual double StandardError { get; set; }
		public virtual double NotOcurenceValue { get; set; }
		public virtual FeatureType Type { get; set; }
		public virtual long? Ascendant { get; set; }
		public virtual bool NeedsToReconstructProductLocationStock { get; set; }
	}
}
