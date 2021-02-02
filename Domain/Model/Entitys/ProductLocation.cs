using Domain.Abstraction.Model;
using Domain.Abstraction.Model.Entitys;
using Domain.Standard.Enums;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using Old._42.SCP.Domain.Bases.Entitys;
using Old._42.Util.Extensions;
using System;
using System.Collections.Generic;

namespace Domain.Model.Entitys
{
    public class ProductLocation : Entity, IProductLocation
    {
        public ProductLocation()
        {
        }

        public ProductLocation(
            long id,
            double quantityOnHand,
            double quantityOnOrder,
            double quantityReserved,
            decimal? defaultLambda,
            double ecomReserved,
            IProduct product,
            ILocation location,
            IProductLocationStats stats,
            DateTime? firstHistory,
            DateTime? lastHistory)
            : this(id, quantityOnHand, quantityOnOrder, quantityReserved, defaultLambda, ecomReserved, product, location, firstHistory, lastHistory)
        {
            Stats = stats;
        }

        public ProductLocation(
            long id,
            double quantityOnHand,
            double quantityOnOrder,
            double quantityReserved,
            decimal? defaultLambda,
            double ecomReserved,
            IProduct product,
            ILocation location,
            DateTime? firstHistory,
            DateTime? lastHistory)
        {
            ID = id;
            QuantityOnHand = quantityOnHand;
            QuantityOnOrder = quantityOnOrder;
            QuantityReserved = quantityReserved;
            EcomReserved = ecomReserved;
            DefaultLambda = defaultLambda;
            Product = product;
            Location = location;
            FirstHistory = firstHistory;
            LastHistory = lastHistory;
        }

        public virtual double QuantityOnHand { get; set; }
        public virtual double QuantityOnOrder { get; set; }
        public virtual double QuantityReserved { get; set; }
        public virtual double EcomReserved { get; set; }
        public virtual decimal? DefaultLambda { get; set; }
        public virtual IProduct Product { get; set; }
        public virtual ILocation Location { get; set; }
        public virtual IProductLocationStats Stats { get; set; } = new ProductLocationStats();
        public virtual IProductLocationStats StableStats { get; set; } = new ProductLocationStats();
        public virtual IProductLocationStats StatsNew { get; set; } = new ProductLocationStats();
        public virtual DateTime? FirstHistory { get; set; }
        public virtual DateTime? LastHistory { get; set; }

        public virtual double AllocatedInventory =>
            QuantityOnHand.OrZeroIfNegative() + QuantityOnOrder + QuantityReserved;

        public virtual double AvailableInventory =>
            (QuantityOnHand - EcomReserved).OrZeroIfNegative();

        public long LocationID =>
            Location.ID;

        public long ProductID =>
            Product.ID;

        public virtual string GetGroupName(IEnumerable<string> productGroupers, IEnumerable<string> locationGroupers)
        {
            string result;

            if (locationGroupers.Empty())
            {
                result = Product.GetGroupName(productGroupers);
            }
            else if (productGroupers.Empty())
            {
                result = Location.GetGroupName(locationGroupers);
            }
            else
            {
                result = string.Join("|", Product.GetGroupName(productGroupers), Location.GetGroupName(locationGroupers));
            }

            return result;
        }

        public virtual string GetGroupName(IRegressionLevel regressionLevel)
        {
            return GetGroupName(regressionLevel.ProductGroupers, regressionLevel.LocationGroupers);
        }

        public virtual void CalculateLambdasForItensWithMinimumHistory(int minBeta)
        {
            StableStats.CalculateLambda(minBeta);
            StatsNew.CalculateLambda(minBeta);
        }

        public virtual void SetStats(
            decimal alfa,
            int beta,
            decimal lambda,
            int daysWithPositiveStock,
            int daysWithSales,
            decimal totalSales,
            LambdaCalculationType lambdaCalculationType)
        {
            var productLocationStats = new ProductLocationStats(alfa, beta, daysWithPositiveStock, daysWithSales, totalSales);
            SetStats(productLocationStats, lambda, lambdaCalculationType);
        }

        public virtual void SetStats(IProductLocationStats productLocationStats, decimal lambda, LambdaCalculationType lambdaCalculationType)
        {
            Stats = productLocationStats;
            Stats.SetLambda(lambda, lambdaCalculationType);
        }

        public virtual void SetStableStats(int beta, int daysWithPositiveStock, int daysWithSales, decimal totalSales)
        {
            var productLocationStats = new ProductLocationStats(beta, daysWithPositiveStock, daysWithSales, totalSales);
            SetStableStats(productLocationStats, 0m, LambdaCalculationType.Invalid);
        }

        public virtual void SetStableStats(IProductLocationStats productLocationStats, decimal lambda, LambdaCalculationType lambdaCalculationType)
        {
            StableStats = productLocationStats;
            StableStats.SetLambda(lambda, lambdaCalculationType);
        }

        public virtual void SetStatsNew(int beta, int daysWithPositiveStock, int daysWithSales, decimal totalSales)
        {
            var productLocationStats = new ProductLocationStats(beta, daysWithPositiveStock, daysWithSales, totalSales);
            SetStatsNew(productLocationStats, 0m, LambdaCalculationType.Invalid);
        }

        public virtual void SetStatsNew(IProductLocationStats productLocationStats, decimal lambda, LambdaCalculationType lambdaCalculationType)
        {
            StatsNew = productLocationStats;
            StatsNew.SetLambda(lambda, lambdaCalculationType);
        }

        public virtual bool HasValidStats() =>
            (Stats.IsNotNull() && Stats.IsValid());

        public virtual void AddDayStats(IHistoryInfoAux aux)
        {
            if (aux.IsStable)
            {
                StableStats.AddHistoryStats(aux);
            }

            StatsNew.AddHistoryStats(aux);
        }
    }
}
