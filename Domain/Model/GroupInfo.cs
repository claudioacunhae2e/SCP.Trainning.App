using Domain.Abstraction.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Model
{
    public class GroupInfo : IGroup
    {
        public GroupInfo()
        {
            FirstSale = DateTime.MaxValue;
            LastSale = DateTime.MinValue;
        }

        public GroupInfo(IGroup groupStats) : this()
        {
            RegressionLevel = groupStats.RegressionLevel;
            Name = groupStats.Name;
            ParentName = groupStats.ParentName;
            DaysWithPositiveStock = groupStats.DaysWithPositiveStock;
            DaysWithSales = groupStats.DaysWithSales;
            SalesQuantity = groupStats.SalesQuantity;
            SalesRevenue = groupStats.SalesRevenue;
            QuantityOnHand = groupStats.QuantityOnHand;
            QuantityOnOrder = groupStats.QuantityOnOrder;
            QuantityReserved = groupStats.QuantityReserved;
            _Histories = groupStats.Histories.Select(h => (GroupHistory)h).ToList();
        }

        public long ProductID { get; }
        public long LocationID { get; }
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
        public string PromotionKey { get; set; }
        public DateTime FirstSale { get; private set; }
        public DateTime LastSale { get; private set; }
        public int SalesAge { get; private set; }
        public double DaysWithSalesOverSalesAge { get; private set; }

        public IList<IGroupHistory> Histories
        {
            get => _Histories.ToList<IGroupHistory>();
            set => _Histories = (List<GroupHistory>)value;
        }

        private List<GroupHistory> _Histories { get; set; }

        #region Not implemented
        public void Add<T>(IProductLocation<T> stock) where T : IHistory
        {
            throw new NotImplementedException();
        }

        public IGroupHistory AddHistory(DateTime date, double quantityOnHand, double revenue, double salesQuantity, double movements)
            => throw new NotImplementedException();

        public IGroupHistory AddHistory(DateTime date, double quantityOnHand)
        {
            throw new NotImplementedException();
        }

        public bool IsMature(int minDaysWithSales, double minDaysWithSalesOverSalesAge)
            => throw new NotImplementedException();
        #endregion
    }
}
