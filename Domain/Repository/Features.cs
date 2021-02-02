using Domain.Abstraction.Repository;
using Domain.Model;
using E2E.Infra.SQL;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using Old._42.SCP.Domain.Abstractions.Bases.Entitys;
using Old._42.SCP.Domain.Bases.Entitys;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Model.Entitys;

namespace Domain.Repository
{
    public class Features : RepositoryBase<FeatureDTO, FeatureDTO>, IFeatureRepository
    {
        public Features(IDataBaseAccess dataBaseAccess) : base("Feature", dataBaseAccess)
        {
        }

        private const string _QueryByRegressionLevel = @"
            SELECT Feature.*
            FROM Feature
            JOIN RegressionLevelFeature ON RegressionLevelFeature.Feature = Feature.ID
            WHERE RegressionLevel = @id  ";

        private const string _QueryById = "SELECT Feature.* FROM Feature WHERE ID = @id";

        public IList<IFeature> ByRegressionLevel(long id)
        {
            var param = new
            {
                id
            };

            var result = Query(_QueryByRegressionLevel, param);
            return result.Select(x => Build(x)).ToList();
        }

        public async Task<IEnumerable<IFeature>> ByRegressionLevelAsync(long id, string scope = null)
        {
            var param = new
            {
                id
            };

            var result = await SqlDapper.QueryAsync<FeatureDTO>(_QueryByRegressionLevel, param);
            return result.Select(x => Build(x)).ToList();
        }

        public new async Task<IFeature> ByIDAsync(long id, string scope = null)
        {
            var param = new
            {
                id
            };

            var result = await SqlDapper.QueryFirstAsync<FeatureDTO>(_QueryById, param);
            return Build(result);
        }

        public IFeature Build(FeatureDTO dto)
        {
            if (dto.Ascendant == null)
                return Build(dto, null);

            var ascendant = Build(ByID(dto.Ascendant ?? 0));

            return Build(dto, ascendant);
        }

        IFeature IReadOnlyRepository<IFeature>.ByID(long id)
        {
            var param = new
            {
                id
            };

            var queryResult = QueryFirst(_QueryById, param);

            return Build(queryResult);
        }

        private IFeature Build(FeatureDTO dto, IFeature Ascendant)
        {
            return new Feature
            {
                ID = dto.ID,
                Name = dto.Name,
                Rule = dto.Rule,
                Coefficient = dto.Coefficient,
                Variation = dto.Variation,
                StandardError = dto.StandardError,
                NotOcurenceValue = dto.NotOcurenceValue,
                Type = dto.Type,
                NeedsToReconstructProductLocationStock = dto.NeedsToReconstructProductLocationStock,
                Ascendant = Ascendant,
            };
        }

        #region Not Implemented
        IList<IFeature> IReadOnlyRepository<IFeature>.All()
        {
            throw new System.NotImplementedException();
        }

        IList<IFeature> IReadOnlyRepository<IFeature>.AllIgnored()
        {
            throw new System.NotImplementedException();
        }

        IFeature IReadOnlyRepository<IFeature>.ByName(string name)
        {
            throw new System.NotImplementedException();
        }

        IList<IFeature> IReadOnlyRepository<IFeature>.ByIDs(long[] ids)
        {
            throw new System.NotImplementedException();
        }

        public IFeature Ignore(IFeature entity)
        {
            throw new System.NotImplementedException();
        }

        IDictionary<long, IFeature> IReadOnlyRepository<IFeature>.AllAsDictionary()
        {
            throw new System.NotImplementedException();
        }

        IDictionary<long, IFeature> IReadOnlyRepository<IFeature>.ByIDsAsDictionary(long[] ids)
        {
            throw new System.NotImplementedException();
        }

        Task<IEnumerable<IFeature>> IReadOnlyRepository<IFeature>.QueryAsync(string query, object parameters)
        {
            throw new System.NotImplementedException();
        }

        Task<IEnumerable<IFeature>> IReadOnlyRepository<IFeature>.AllAsync(string scope = null)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
