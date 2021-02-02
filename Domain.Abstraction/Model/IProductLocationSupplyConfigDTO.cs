using Domain.Standard.Enums;
using System;

namespace Domain.Abstraction.Model
{
    public interface IProductLocationSupplyConfigDTO
    {

        #region Product Attributes

        string ProductClientID { get; set; }
        string RegressionModelName { get; set; }

        #endregion


        #region Location Attributes

        string LocationClientID { get; set; }
        string DistributionCenterClientID { get; set; }

        #endregion


        #region SupplyConfig Attributes

        decimal SLA { get; set; }

        bool Supply { get; set; }

        TypeOfSupply SupplyType { get; set; }

        EchelonLevel EchelonLevel { get; set; }

        short LeadTime { get; set; }

        short ReviewPeriod { get; set; }

        string OrigemLeadTimeReviewPeriod { get; set; }

        string OrigemSLA { get; set; }

        int? MinDisplay { get; set; }

        string OrigemMinDisplay { get; set; }

        int? MaxTarget { get; set; }

        string OrigemMaxTarget { get; set; }

        int? AdditionalCoverageDays { get; set; }

        string OrigemAdditionalCoverageDays { get; set; }

        int? MinimumTarget { get; set; }

        DateTime? MinimumTargetStart { get; set; }

        DateTime? MinimumTargetEnd { get; set; }

        bool MinimumTargetEnabled { get; set; }

        #endregion

    }
}
