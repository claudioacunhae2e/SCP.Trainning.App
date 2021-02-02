using Domain.Abstraction.Factory;
using Domain.Abstraction.Model;
using Domain.Abstraction.Model.Entitys;
using Domain.Abstraction.Service;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using Old._42.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Model
{
    public class ProductLocationInfo : IProductLocationInfo
    {
        public ProductLocationInfo(IProductLocation productLocation, string promotionKeyAttribute)
        {
            ProductLocation = productLocation;
            QuantityOnHand = productLocation.QuantityOnHand;
            QuantityOnOrder = productLocation.QuantityOnOrder;
            QuantityReserved = productLocation.QuantityReserved;
            Product = productLocation.Product;
            Location = productLocation.Location;
            ProductID = Product.ID;
            LocationID = Location.ID;
            PromotionKey = Product.Attributes[promotionKeyAttribute];
            Histories = new List<IHistoryInfo>();
        }

        public IProductLocation ProductLocation { get; }
        public IProduct Product { get; }
        public ILocation Location { get; }
        public double QuantityOnHand { get; }
        public double QuantityOnOrder { get; }
        public double QuantityReserved { get; }
        public long ProductID { get; }
        public long LocationID { get; }
        public string PromotionKey { get; }
        public IList<IHistoryInfo> Histories { get; set; }

        public IHistoryInfo AddHistory(DateTime date, double quantityOnHand, double revenue, double salesQuantity, double movements)
        {
            var info = new HistoryInfo(date, movements, quantityOnHand, revenue, salesQuantity);
            Histories.Add(info);
            return info;
        }

        public IHistoryInfo AddHistory(DateTime date, double movements, double revenue, double salesQuantity)
        {
            var info = new HistoryInfo(date, movements, revenue, salesQuantity);
            Histories.Add(info);
            return info;
        }

        public string GetGroupName(IEnumerable<string> productGroupers, IEnumerable<string> locationGroupers) =>
            ProductLocation.GetGroupName(productGroupers, locationGroupers);

        public string GetGroupName(IRegressionLevel regressionLevel) =>
            ProductLocation.GetGroupName(regressionLevel);

        public void CalculateLambda(IAlfaCalculator normalizer, IFeatureEngineerFactory factory, int minBeta, DateTime stability)
        {
            var historysWithSales = CalculateLambdaGetHistories();

            if (historysWithSales.Any())
            {
                var engineer = GetInputFeatureEngineer(factory, historysWithSales);
                var aux = new ProductLocationInfoCalculateLambda(engineer, stability, minBeta);

                CalculateLambdaExec(normalizer, historysWithSales, aux);
                CalculateLambdaSetProductLocationStats(aux);
            }
        }

        private void CalculateLambdaExec(IAlfaCalculator normalizer, IEnumerable<IHistoryInfo> historysWithSales, ProductLocationInfoCalculateLambda aux)
        {
            foreach (var history in historysWithSales)
            {
                var value = normalizer.Normalize(history.SalesQuantity, history.Date, aux.Engineer);

                aux.Alfa += value;
                if (history.Date <= aux.Stability)
                {
                    aux.StableAlfa += value;
                }
            }
        }

        private void CalculateLambdaSetProductLocationStats(ProductLocationInfoCalculateLambda aux)
        {
            if (ProductLocation.StableStats.IsNull())
            {
                ProductLocation.SetStableStats(0, 0, 0, 0m);
            }

            ProductLocation.Stats = SetStatsAlfa(ProductLocation.Stats, (decimal)aux.Alfa, aux.MinBeta);
            ProductLocation.StableStats = SetStatsAlfa(ProductLocation.StableStats, (decimal)aux.StableAlfa, aux.MinBeta);
        }

        private IEnumerable<IHistoryInfo> CalculateLambdaGetHistories()
        {
            return Histories.Any()
                    ? Histories.Where(h => h.SalesQuantity > 0)
                    : new List<IHistoryInfo>();
        }

        private IInputFeatureEngineer GetInputFeatureEngineer(IFeatureEngineerFactory factory, IEnumerable<IHistoryInfo> historysWithSales)
        {
            var minDate = historysWithSales.Min(h => h.Date);
            var maxDate = historysWithSales.Max(h => h.Date);
            var timeScope = new TimeScope(minDate, maxDate);

            var engineer = factory.Get(timeScope, LocationID);
            return engineer;
        }

        private IProductLocationStats SetStatsAlfa(IProductLocationStats stats, decimal alfa, int minBeta)
        {
            IProductLocationStats result;

            if (stats.IsNull())
            {
                result = new ProductLocationStats(alfa, 0, 0, 0, 0m);
            }
            else
            {
                stats.SetAlfaAndCalculateLambda(alfa, minBeta);
                result = stats;
            }

            return result;
        }
    }
}
