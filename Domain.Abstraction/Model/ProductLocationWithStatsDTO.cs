using Domain.Abstraction.Model.Entitys;
using Domain.Standard.Enums;

namespace Domain.Abstraction.Model
{
    public class ProductLocationWithStatsDTO : IProductLocationWithStats
    {
        public ProductLocationWithStatsDTO(IProductLocationStats stats, IProductLocationStats stableStats, long id, long locationId, long productId)
        {
            _Stats = (ProductLocationStats)stats;
            _StableStats = (ProductLocationStats)stableStats;
            ID = id;
            LocationID = locationId;
            ProductID = productId;
        }

        public ProductLocationWithStatsDTO(IProductLocationWithStats productLocationWithStats)
            : this(productLocationWithStats.Stats, productLocationWithStats.StableStats, productLocationWithStats.ID, productLocationWithStats.LocationID, productLocationWithStats.ProductID)
        {
        }

        public ProductLocationWithStatsDTO(IProductLocation productLocation)
            : this(productLocation.Stats, productLocation.StableStats, productLocation.ID, productLocation.LocationID, productLocation.ProductID)
        {
        }

        public long LocationID { get; set; }
        public long ProductID { get; set; }
        public long ID { get; set; }
        public string Name { get; }

        public IProductLocationStats Stats
        {
            get => _Stats;
            set => _Stats = (ProductLocationStats)value;
        }

        public IProductLocationStats StableStats
        {
            get => _StableStats;
            set => _Stats = (ProductLocationStats)value;
        }

        private ProductLocationStats _Stats { get; set; }
        private ProductLocationStats _StableStats { get; set; }

        public void SetStats(IProductLocationStats productLocationStats, decimal lambda, LambdaCalculationType lambdaCalculationType)
        {
            Stats = productLocationStats;
            Stats.SetLambda(lambda, lambdaCalculationType);
        }
    }
}
