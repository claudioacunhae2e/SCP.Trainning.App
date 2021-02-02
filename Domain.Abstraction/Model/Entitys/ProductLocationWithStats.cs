using Domain.Standard.Enums;
using Old._42.SCP.Domain.Bases.Entitys;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Abstraction.Model.Entitys
{
    public class ProductLocationWithStats : Entity, IProductLocationWithStats
    {
        public ProductLocationWithStats(IProductLocation productLocation)
        {
            LocationID = productLocation.LocationID;
            ProductID = productLocation.ProductID;
            Stats = productLocation.Stats;
            StableStats = productLocation.StableStats;
            ID = productLocation.ID;
        }

        public long LocationID { get; set; }
        public long ProductID { get; set; }
        public virtual IProductLocationStats Stats { get; set; }
        public virtual IProductLocationStats StableStats { get; set; }

        public void SetStats(IProductLocationStats productLocationStats, decimal lambda, LambdaCalculationType lambdaCalculationType)
        {
            Stats = productLocationStats;
            Stats.SetLambda(lambda, lambdaCalculationType);
        }
    }
}
