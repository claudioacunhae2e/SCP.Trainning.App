using E2E.SCP.RegressionModel.Abstraction.Model;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using E2E.SCP.RegressionModel.Model;
using E2E.SCP.RegressionModel.Standard.Enum;
using Old._42.SCP.Domain.Bases.Entitys;
using System.Runtime.Serialization;

namespace Domain.Model.Entitys
{
    [DataContract]
    [KnownType("GetKnownTypes")]
    public class Feature : Entity, IFeature
    {
        [DataMember]
        public virtual string Rule { get; set; }

        [DataMember]
        public virtual double Coefficient { get; set; }

        [DataMember]
        public virtual double Variation { get; set; }

        [DataMember]
        public virtual double StandardError { get; set; }

        [DataMember]
        public virtual double NotOcurenceValue { get; set; }

        [DataMember]
        public virtual FeatureType Type { get; set; }

        [DataMember]
        public virtual IFeature Ascendant { get; set; }

        [DataMember]
        public virtual bool NeedsToReconstructProductLocationStock { get; set; }

        public virtual ICoefficient DeafultCoefficient() =>
            new Coefficient(Coefficient, Variation, StandardError);
    }
}
