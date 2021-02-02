using Domain.Abstraction.Model;
using Domain.Standard.Enums;
using System;

namespace Domain.Model
{
    public class ProductLocationSupplyConfigDTO : IProductLocationSupplyConfigDTO
    {

        #region Product Attributes

        public string ProductClientID { get; set; }
        public string RegressionModelName { get; set; }

        #endregion


        #region Location Attributes

        public string LocationClientID { get; set; }
        public string DistributionCenterClientID { get; set; }

        #endregion


        #region SupplyConfig Attributes

        public decimal SLA { get; set; }

        public bool Supply { get; set; }

        public TypeOfSupply SupplyType { get; set; }

        public EchelonLevel EchelonLevel { get; set; }

        public short LeadTime { get; set; }

        public short ReviewPeriod { get; set; }

        public string OrigemLeadTimeReviewPeriod { get; set; }

        public string OrigemSLA { get; set; }

        public int? MinDisplay { get; set; }

        public string OrigemMinDisplay { get; set; }

        public int? MaxTarget { get; set; }

        public string OrigemMaxTarget { get; set; }

        public int? AdditionalCoverageDays { get; set; }

        public string OrigemAdditionalCoverageDays { get; set; }

        public int? MinimumTarget { get; set; }

        public DateTime? MinimumTargetStart { get; set; }

        public DateTime? MinimumTargetEnd { get; set; }

        public bool MinimumTargetEnabled { get; set; }

        #endregion

    }
}
