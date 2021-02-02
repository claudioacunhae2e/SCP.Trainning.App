using Domain.Abstraction.Repository;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using E2E.SCP.RegressionModel.Model.Entitys;
using Old._42.SCP.Domain.Abstractions.Bases.Entitys;
using Old._42.SCP.Domain.Bases.Entitys;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class RegressionLevels : RepositoryBase<RegressionLevel, IRegressionLevel>, IRegressionLevelRepository
    {
        public RegressionLevels(IDataBaseAccess dataBaseAccess, IFeatureRepository features) : base(dataBaseAccess)
        {
            _Features = features;
        }

        private readonly string _QueryBase = @"
				SELECT  [ID]					[ID]
					   ,[Name]					[Name]
					   ,[Configs]				[Configs]
					   ,[Ascendant]				[AscendantID]
					   ,[LocationGroupersText]	[LocationGroupersText]
					   ,[ProductGroupersText]	[ProductGroupersText]
					   ,[GroupByProduct]		[GroupByProduct]
					   ,[GroupByLocation]		[GroupByLocation]
					   ,[Train]					[Train]
					   ,[Order]					[Order]
				  FROM RegressionLevel  ";

        private readonly IFeatureRepository _Features;

        protected override string BaseSelectQuery() =>
            _QueryBase;

        public override IList<IRegressionLevel> All()
        {
            var levels = base.All();

            foreach (var level in levels)
            {
                level.SetFeatures(_Features.ByRegressionLevel(level.ID));
            }

            return levels;
        }

        public override IRegressionLevel ByID(long id)
        {
            var level = base.ByID(id);
            level.SetFeatures(_Features.ByRegressionLevel(level.ID));

            return level;
        }

        public override async Task<IRegressionLevel> ByIDAsync(long id, string scope)
        {
            var level = await base.ByIDAsync(id, scope);
            var features = await _Features.ByRegressionLevelAsync(level.ID, scope);

            level.SetFeatures(features.ToList());

            return level;
        }
    }
}
