using Domain.Standard.Enums;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using Old._42.SCP.Domain.Abstractions.Bases.Entitys;
using System;
using System.Collections.Generic;

namespace Domain.Abstraction.Model.Entitys
{
    public interface IProductLocation : IEntity, ICanHaveMyLambdaReplacedBySimilar
    {
        IProductLocationStats Stats { get; set; }
        IProductLocationStats StableStats { get; set; }
        IProductLocationStats StatsNew { get; set; }
        double QuantityOnHand { get; }
        double QuantityOnOrder { get; }
        double QuantityReserved { get; }
        double EcomReserved { get; }
        double AllocatedInventory { get; }
        double AvailableInventory { get; }
        DateTime? FirstHistory { get; }
        DateTime? LastHistory { get; }
        decimal? DefaultLambda { get; }
        bool HasValidStats();
        IProduct Product { get; }
        ILocation Location { get; }

        void SetStats(decimal alfa, int beta, decimal lambda, int daysWithPositiveStock, int daysWithSales, decimal totalSales, LambdaCalculationType lambdaCalculationType);
        void SetStats(IProductLocationStats productLocationStats, decimal lambda, LambdaCalculationType lambdaCalculationType);
        string GetGroupName(IEnumerable<string> productGroupers, IEnumerable<string> locationGroupers);
        string GetGroupName(IRegressionLevel regressionLevel);
        void SetStableStats(IProductLocationStats productLocationStats, decimal lambda, LambdaCalculationType lambdaCalculationType);
        void SetStableStats(int beta, int daysWithPositiveStock, int daysWithSales, decimal totalSales);
        void SetStatsNew(IProductLocationStats productLocationStats, decimal lambda, LambdaCalculationType lambdaCalculationType);
        void SetStatsNew(int beta, int daysWithPositiveStock, int daysWithSales, decimal totalSales);
        void CalculateLambdasForItensWithMinimumHistory(int minBeta);
        void AddDayStats(IHistoryInfoAux aux);
    }
}
