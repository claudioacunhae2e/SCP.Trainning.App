using Domain.Abstraction.Repository;
using E2E.Infra.SQL;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using E2E.SCP.RegressionModel.Model.Entitys;
using Microsoft.Extensions.Logging;
using Old._42.SCP.Domain.Abstractions.Bases.Entitys;
using Old._42.SCP.Domain.Bases.Entitys;
using Old._42.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class RegressionModels : RepositoryBase<RegressionModel, IRegressionModel>, IRegressionModelRepository
    {
        public RegressionModels(
            IDataBaseAccess dataBaseAccess,
            ITempTableCreator tempTableCreator,
            ILogger<RegressionModels> logger)
            : base(dataBaseAccess)
        {
            _TempTableCreator = tempTableCreator;
            _Logger = logger;
            _Entities = new Dictionary<long, IDictionary<string, IRegressionModel>>();
        }

        private const string _QueryBase = @"
                  SELECT [ID]
					  ,[Key]			Name
					  ,[ModelJSON]
					  ,[RegressionLevel] [RegressionLevel]
					  ,[Trained]
				  FROM [dbo].[RegressionModel]  WITH (NOLOCK) "; //TODO: remover filtr WHERE [Key] = '2|130|130661'  WHERE [RegressionLevel] = 1

        private const string _QueryBaseInsert = @"
                INSERT INTO {0} ([Key], [ModelJSON], [RegressionLevel], [Trained])
                VALUES (@name, @modelJSON, @regressionLevel, @itemID, @trained)    ";

        private const string _QueryUpdateCommad = @"
				MERGE  RegressionModel     AS TargetTable
				USING #{0} AS SourceTable ON TargetTable.RegressionLevel = SourceTable.RegressionLevel 
														 AND TargetTable.[Key] = SourceTable.[Key]
				 WHEN MATCHED THEN
					UPDATE SET
							 TargetTable.[ModelJSON]    = SourceTable.[ModelJSON]
							,TargetTable.[Trained]      = SourceTable.[Trained]
				 WHEN NOT MATCHED THEN
					INSERT (    [Key]
							,   [ModelJSON]
							,   [RegressionLevel]
							,   [Trained])
					VALUES(     SourceTable.[Key]
						    ,   SourceTable.[ModelJSON]
						    ,   SourceTable.[RegressionLevel]
						    ,   SourceTable.[Trained]);     ";

        private readonly ITempTableCreator _TempTableCreator;
        private readonly ILogger<RegressionModels> _Logger;
        private readonly IDictionary<long, IDictionary<string, IRegressionModel>> _Entities;

        protected override string BaseSelectQuery() =>
            _QueryBase;

        protected override string BaseInsertQuery() =>
           string.Format(_QueryBaseInsert, Table);

        public async Task Update(IRegressionModel model)
        {
            _Logger.LogInformation(string.Concat("Start Update model of item : ", model.Name));

            var tableid = Guid.NewGuid().ToString("N");
            var query = string.Format(string.Format(_QueryUpdateCommad, tableid));
            var tempTable = CreateTempTable(tableid);

            await UpdateExec(query, model, tempTable);

            _Logger.LogInformation(string.Concat("Start Update model of item : ", model.Name));
        }

        public void Add(IRegressionModel regressionModel)
        {
            var velue = new Dictionary<string, IRegressionModel>();
            var models = _Entities.AddIfNotExistsAndGet(regressionModel.RegressionLevel, velue);

            models.AddOrUpdate(regressionModel.Name, regressionModel);
        }

        public void Add(IEnumerable<IRegressionModel> regressionModels)
        {
            foreach (var regressionModel in regressionModels)
            {
                Add(regressionModel);
            }
        }

        public IEnumerable<IRegressionModel> ByLevel(long levelID)
        {
            if (_Entities.Empty())
            {
                Add(All());
            }

            var existsLevels = _Entities.GetIfExists(levelID);
            return existsLevels.Select(e => e.Value);
        }

        public async Task<IEnumerable<IRegressionModel>> ByLevelAsync(long levelID, string scope = null)
        {
            var result = await AllAsync(scope);
            Add(result);

            return result;
        }

        public async Task<IEnumerable<IRegressionModel>> ByLevelAsync(long levelID, IEnumerable<string> keys)
        {
            var result = new List<IRegressionModel>();
            string query = ByLevelPaginatedAsyncGetQuery(levelID, keys);

            var resultQuery = await SqlDapper.QueryAsync<RegressionModel>(query);

            result.AddRange(resultQuery);
            Add(result);

            return result;
        }

        public override IRegressionModel Save(IRegressionModel entity)
        {
            var query = string.Format(_QueryBaseInsert, Table);
            var param = new
            {
                name = entity.Name,
                modelJSON = entity.ModelJSON,
                RegressionLevel = entity.RegressionLevel,
                trained = entity.Trained
            };

            SqlDapper.Execute(query, param);

            return entity;
        }

        private async Task UpdateExec(
            string query,
            IRegressionModel model,
            ITempTableDescription<IRegressionModel> tempTable)
        {
            using (var connection = await DataBaseAccess.GetAsync())
            {
                await tempTable.Insert(model, connection);

                using (var command = connection.CreateCommand())
                {
                    command.CommandTimeout = 0;
                    command.CommandText = query;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        private ITempTableDescription<IRegressionModel> CreateTempTable(string tableId)
        {
            return _TempTableCreator
                        .Create<IRegressionModel>(tableId)
                        .AddField<string>("Key", "NVarChar(50) COLLATE Latin1_General_CI_AI", r => r.Name)
                        .AddField<string>("ModelJSON", "NVarChar(max) COLLATE Latin1_General_CI_AI", r => r.ModelJSON)
                        .AddField<long>("RegressionLevel", "bigint", r => r.RegressionLevel)
                        .AddField<bool>("Trained", "bit", r => r.Trained);
        }

        private string ByLevelPaginatedAsyncGetQuery(long levelID, IEnumerable<string> keys)
        {
            var query = string.Concat(_QueryBase, "\n\tWHERE [RegressionLevel] = ", levelID);
            if (keys.Count() < 100) //TO-DO: feito para funcionar temporariamente no debug
            {
                var join = string.Join("','", keys);
                query = string.Format("{0}\tand [Key] IN ('{1}')", query, join);
            }

            return query;
        }
    }
}
