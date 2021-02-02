using Domain.Abstraction.Model;
using System;
using System.Collections.Generic;

namespace Domain.Model
{
    public class Group : IGroup
    {
        public Group()
        {

        }

        public Group(long levelID, string name, string parentName, IList<IGroupHistory> histories)
        {
            RegressionLevel = levelID;
            Name = name;
            ParentName = parentName;
            Histories = histories;
        }

        public Group(long regressionLevel, IGroupNameDTO parentModel)
        {
            RegressionLevel = regressionLevel;
            Name = parentModel.Name;
            ParentName = parentModel.ParentName;
        }

        private readonly object _LockIncrementInventory = new object();

        public long RegressionLevel { get; set; }
        public string Name { get; set; }
        public string ParentName { get; set; }
        public int DaysWithPositiveStock { get; set; }
        public int DaysWithSales { get; set; }
        public double SalesQuantity { get; set; }
        public double SalesRevenue { get; set; }
        public double QuantityOnHand { get; set; }
        public double QuantityOnOrder { get; set; }
        public double QuantityReserved { get; set; }
        public DateTime FirstSale { get; private set; } = DateTime.MaxValue;
        public DateTime LastSale { get; private set; } = DateTime.MinValue;
        public int SalesAge { get; private set; }
        public double DaysWithSalesOverSalesAge { get; private set; }
        public IList<IGroupHistory> Histories
        {
            get => _Histories;
            set => SetHistories(value);
        }

        private IList<IGroupHistory> _Histories { get; set; }

        public bool IsMature(int minDaysWithSales, double minDaysWithSalesOverSalesAge) =>
            DaysWithSales >= minDaysWithSales && DaysWithSalesOverSalesAge >= minDaysWithSalesOverSalesAge;

        public void IncrementInventory<T>(IProductLocation<T> productlocation) where T : IHistory
        {
            lock (_LockIncrementInventory)
            {
                QuantityOnHand += productlocation.QuantityOnHand;
                QuantityOnOrder += productlocation.QuantityOnOrder;
                QuantityReserved += productlocation.QuantityReserved;
            }
        }

        public IGroupHistory AddHistory(DateTime date, double quantityOnHand, double revenue, double salesQuantity, double movements)
        {
            var item = new GroupHistory(date, quantityOnHand, revenue, salesQuantity, movements);
            Histories.Add(item);

            return item;
        }

        private void SetHistories(IList<IGroupHistory> histories)
        {
            SetHistoriesByItem(histories);

            _Histories = histories;
            SalesAge = (LastSale - FirstSale).Days;

            DaysWithSalesOverSalesAge = SalesAge > 0
                                        ? ((double)DaysWithSales / SalesAge)
                                        : 0d;
        }

        private void SetHistoriesByItem(IList<IGroupHistory> histories)
        {
            foreach (var history in histories)
            {
                if (history.SalesRevenue > 0)
                {
                    SetHistoriesByItemSalesRevenuePositive(history);
                }
                else
                {
                    if (history.QuantityOnHand > 0)
                    {
                        DaysWithPositiveStock++;
                    }
                }
            }
        }

        private void SetHistoriesByItemSalesRevenuePositive(IGroupHistory history)
        {
            if (history.Date < FirstSale)
            {
                FirstSale = history.Date;
            }

            if (history.Date > LastSale)
            {
                LastSale = history.Date;
            }

            DaysWithPositiveStock++;
            DaysWithSales++;
            SalesRevenue += history.SalesRevenue;
            SalesQuantity += history.SalesQuantity;
        }

        #region Not implemented
        public long ProductID => throw new NotImplementedException();
        public long LocationID => throw new NotImplementedException();
        public string PromotionKey => throw new NotImplementedException();
        #endregion
    }
}
