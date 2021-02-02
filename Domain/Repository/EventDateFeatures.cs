using Dapper;
using Domain.Abstraction.Model.Entitys;
using Domain.Abstraction.Repository;
using Domain.Model.Entitys;
using Old._42.SCP.Domain.Abstractions.Bases.Entitys;
using Old._42.SCP.Domain.Bases.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class EventDateFeatures : RepositoryBase<EventDateFeature, IEventDateFeature>, IEventDateFeatureRepository
    {
        public EventDateFeatures(
            IDataBaseAccess dataBaseAccess,
            IFeatureRepository features) : base(dataBaseAccess)
        {
            Features = features;
        }

        private const string _QueryBase = @"
					SELECT [ID]					[ID]
						  ,[Start]				[Start]
						  ,[End]				[End]
						  ,[OcurrenceValue]		[OcurrenceValue]
						  ,[Feature]			[FeatureID]
						  ,[RegressionLevel]	[RegressionLevelID]
					 FROM [dbo].[EventDateFeature]	";

        private readonly IFeatureRepository Features;

        protected override string BaseSelectQuery() =>
            _QueryBase;

        public async Task<IEnumerable<IEventDateFeature>> By(DateTime start, DateTime end, string scope = "")
        {
            IEnumerable<IEventDateFeature> result;

            using (var connection = await DataBaseAccess.GetByScopeAsync(scope))
            {
                var query = BaseFilterQuery("[Start] >= @start AND [End] <= @end");
                var param = new
                {
                    start,
                    end
                };

                result = await connection.QueryAsync<EventDateFeature>(query, param);
            }

            return result;
        }

        public async Task<IList<IEventDateFeature>> ByComponetized(DateTime start, DateTime end, string scope = "")
        {
            var entities = await By(start, end, scope);

            foreach (var entity in entities)
            {
                entity.Feature = await Features.ByIDAsync(entity.FeatureID, scope);
            }

            return entities.ToList();
        }
    }
}
