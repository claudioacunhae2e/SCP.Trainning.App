using Domain.Abstraction.Model;
using Domain.Abstraction.Model.Entitys;
using Domain.Model.Entitys;
using Domain.Standard.Enums;
using Old._42.SCP.Domain.Abstractions.Bases.Entitys;
using Old._42.Util.Extensions;
using System;

namespace Domain.Model
{
    public class ProductLocationDTO : IHasID
    {
        public virtual long ID { get; set; }

        public virtual long Product { get; set; }

        public virtual long Location { get; set; }

        public virtual double QuantityOnHand { get; set; }
        public virtual double QuantityOnOrder { get; set; }
        public virtual double QuantityReserved { get; set; }
        public virtual double EcomReserved { get; set; }

        public virtual decimal? DefaultLambda { get; set; }
        public virtual decimal? Alfa { get; set; }
        public virtual int? Beta { get; set; }
        public virtual decimal? Lambda { get; set; }
        public virtual int? DaysWithPositiveStock { get; }
        public virtual int? DaysWithSales { get; }
        public virtual decimal? TotalSales { get; }
        public virtual int? LambdaCalculationType { get; }
        public virtual int? StableBeta { get; set; }
        public virtual decimal? StableAlfa { get; set; }
        public virtual int? StableDaysWithPositiveStock { get; }
        public virtual int? StableDaysWithSales { get; }
        public virtual decimal? StableTotalSales { get; }
        public virtual DateTime? FirstHistory { get; set; }
        public virtual DateTime? LastHistory { get; set; }


        public double Quantity => QuantityOnHand;

        public IProductLocation Build(IProduct product, ILocation location)
        {
            ProductLocationStats stats = null;
            if (Lambda.HasValue)
            {
                stats = new ProductLocationStats(Alfa.Value, Beta.Value, DaysWithPositiveStock.Value, DaysWithSales.Value, TotalSales.Value);
                stats.SetLambda(Lambda.Value, (LambdaCalculationType)LambdaCalculationType.Value);
            }

            var productLocation = new ProductLocation(
                        ID,
                        QuantityOnHand,
                        QuantityOnOrder,
                        QuantityReserved,
                        DefaultLambda,
                        EcomReserved,
                        product,
                        location,
                        stats,
                        FirstHistory,
                        LastHistory
                        );


            if (StableBeta.IsNotNull())
            {
                var stableStats = new ProductLocationStats(StableAlfa.Value, StableBeta.Value, StableDaysWithPositiveStock.Value, StableDaysWithSales.Value, StableTotalSales.Value);
                productLocation.SetStableStats(stableStats, 0, Domain.Standard.Enums.LambdaCalculationType.Invalid);
            }

            return productLocation;
        }
    }
}
