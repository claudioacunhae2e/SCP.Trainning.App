using Domain.Standard.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Abstraction.Model
{
    public class ProductLocationStats : IProductLocationStats
    {
        public ProductLocationStats()
        {

        }

        public ProductLocationStats(decimal alfa, int beta, int daysWithPositiveStock, int daysWithSales, decimal totalSales)
        {
            Alfa = alfa;
            Beta = beta;
            OpenStoreDaysWithPositiveStock = daysWithPositiveStock;
            DaysWithSales = daysWithSales;
            TotalSales = totalSales;
            LambdaType = LambdaCalculationType.Invalid;
        }

        public ProductLocationStats(int beta, int daysWithPositiveStock, int daysWithSales, decimal totalSales)
        {
            Beta = beta;
            OpenStoreDaysWithPositiveStock = daysWithPositiveStock;
            DaysWithSales = daysWithSales;
            TotalSales = totalSales;
        }

        private readonly object _LockAddHistoryStats = new object();

        public virtual decimal Lambda { get; set; }
        public virtual int Beta { get; set; }
        public virtual decimal Alfa { get; set; }
        public virtual int OpenStoreDaysWithPositiveStock { get; set; }
        public virtual int DaysWithSales { get; set; }
        public virtual decimal TotalSales { get; set; }
        public virtual LambdaCalculationType LambdaType { get; set; }

        public virtual bool IsValid() => Alfa > 0 && LambdaType != LambdaCalculationType.Invalid;

        public virtual void SetLambda(decimal lambda, LambdaCalculationType lambdaCalculationType)
        {
            Lambda = lambda;
            LambdaType = lambdaCalculationType;
        }

        public virtual void CalculateLambda(int minBeta)
        {
            if (Alfa > 0 && Beta > 0)
            {
                if (Beta < minBeta)
                {
                    Lambda = Alfa / minBeta;
                    LambdaType = LambdaCalculationType.MinValue;
                }
                else
                {
                    Lambda = Alfa / Beta;
                    LambdaType = LambdaCalculationType.ProductLocation;
                }
            }
            else
            {
                LambdaType = LambdaCalculationType.Invalid;
            }
        }

        public virtual void SetAlfaAndCalculateLambda(decimal alfa, int minBeta)
        {
            Alfa = alfa;
            CalculateLambda(minBeta);
        }

        public virtual void AddHistoryStats(IHistoryInfoAux historyInfo)
        {
            lock (_LockAddHistoryStats)
            {
                if (historyInfo.HasSales)
                {
                    DaysWithSales++;
                    TotalSales += historyInfo.SalesQuantity;

                    Beta++;
                    OpenStoreDaysWithPositiveStock++;

                    Alfa += historyInfo.Alfa;
                }
                else
                {
                    if (historyInfo.IsValidBeta)
                    {
                        Beta++;
                        OpenStoreDaysWithPositiveStock++;
                    }
                }
            }
        }
    }
}
