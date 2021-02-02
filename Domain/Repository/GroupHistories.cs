using Domain.Abstraction.Model;
using Domain.Abstraction.Repository;
using Domain.Model;
using E2E.Generic.Extension;
using E2E.Infra.SQL;
using Microsoft.Extensions.Logging;
using Old._42.SCP.Domain.Abstractions.Bases.Entitys;
using Old._42.Util.Extensions;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class GroupHistories : IGroupHistoryRepository
    {
        public GroupHistories(
            IDataBaseAccess dataBaseAccess,
            ILogger<GroupHistories> logger)
        {
            _DataBaseAccess = dataBaseAccess;
            _Logger = logger;
        }

        private const int _TimeOut = 0;
        private const string _QueryBase = @"
			  SELECT 
					 GroupHistory.[ID],
					 GroupHistory.[RegressionLevel],
					 GroupHistory.[Name],
					 GroupHistory.[ParentName],
					 GroupHistory.[Date],
					 GroupHistory.[InventoryMovements],
					 GroupHistory.[SalesRevenue],
					 GroupHistory.[SalesQuantity],
					 GroupHistory.[QuantityOnHand],
					 GroupHistory.[Price],
					 GroupHistory.[DistinctStocks],
					 GroupHistory.[OpenStoresWithStock],
                     GroupHistory.[Observed],
					 GroupHistory.[SoftPromotion],
					 GroupHistory.[MediumPromotion],
					 GroupHistory.[IntensePromotion]
				FROM GroupHistory ";

        private const string _QueryUpdateGroupHistory = @"  MERGE GroupHistory
							  USING GroupHistoryTmp Unstable  ON GroupHistory.Name = Unstable.Name
														 AND GroupHistory.ParentName = Unstable.ParentName
														 AND GroupHistory.Date = Unstable.Date
														 AND GroupHistory.RegressionLevel = Unstable.RegressionLevel
							   WHEN MATCHED THEN
									UPDATE SET 
											GroupHistory.InventoryMovements		= Unstable.InventoryMovements
										   ,GroupHistory.SalesRevenue			= Unstable.SalesRevenue
										   ,GroupHistory.SalesQuantity			= Unstable.SalesQuantity
										   ,GroupHistory.QuantityOnHand			= Unstable.QuantityOnHand
										   ,GroupHistory.Price					= Unstable.Price
										   ,GroupHistory.DistinctStocks			= Unstable.DistinctStocks
										   ,GroupHistory.OpenStoresWithStock	= Unstable.OpenStoresWithStock
                                           ,GroupHistory.Observed	            = Unstable.Observed
										   ,GroupHistory.SoftPromotion			= Unstable.SoftPromotion
										   ,GroupHistory.MediumPromotion		= Unstable.MediumPromotion
										   ,GroupHistory.IntensePromotion		= Unstable.IntensePromotion
								WHEN NOT MATCHED THEN  
									INSERT (RegressionLevel,
                                            Name,
                                            ParentName,
                                            Date,
                                            InventoryMovements,
                                            SalesRevenue,
                                            SalesQuantity,
                                            QuantityOnHand,
                                            Price,
                                            DistinctStocks,
                                            OpenStoresWithStock,
                                            Observed,
                                            SoftPromotion,
                                            MediumPromotion,
                                            IntensePromotion)
									VALUES ( Unstable.RegressionLevel
											,Unstable.Name
											,Unstable.ParentName
											,Unstable.Date
											,Unstable.InventoryMovements
											,Unstable.SalesRevenue
											,Unstable.SalesQuantity
											,Unstable.QuantityOnHand
											,Unstable.Price
											,Unstable.DistinctStocks
											,Unstable.OpenStoresWithStock
                                            ,Unstable.Observed
											,Unstable.SoftPromotion
											,Unstable.MediumPromotion
											,Unstable.IntensePromotion);   ";

        private const string _QueryGroupNamesToTrainQuery =
            @"  SELECT DISTINCT 
                     GroupHistory.Name
                    ,GroupHistory.ParentName
			    FROM GroupHistory
			    WHERE       GroupHistory.Observed = 0 
				        AND QuantityOnHand > 0
				        AND GroupHistory.RegressionLevel = @levelID ";

        private readonly string[] _Columns = new string[15]
        {
                    "RegressionLevel",
                    "Name",
                    "Date",
                    "ParentName",
                    "InventoryMovements",
                    "SalesRevenue",
                    "SalesQuantity",
                    "QuantityOnHand",
                    "Price",
                    "DistinctStocks",
                    "OpenStoresWithStock",
                    "Observed",
                    "SoftPromotion",
                    "MediumPromotion",
                    "IntensePromotion",
        };

        private readonly IDataBaseAccess _DataBaseAccess;
        private readonly ILogger<GroupHistories> _Logger;

        public IGroup By(long levelID, string name)
        {
            var query = string.Concat(_QueryBase, "Where GroupHistory.[RegressionLevel] = @levelID AND GroupHistory.[name] = @name");
            var param = new
            {
                levelID,
                name
            };

            return Query(query, param).FirstOrDefault();
        }

        public async Task Save(params IGroup[] groups)
        {
            if (groups.Any())
            {
                await SaveExecute(groups);
            }
        }

        public async Task<IEnumerable<IParentName>> GetGroupNamesToTrain(long levelID)
        {

            var param = new
            {
                levelID
            };
            var result = await SqlDapper.QueryAsync<ParentNameModel>(_QueryGroupNamesToTrainQuery, param, commandTimeout: _TimeOut);

            return result;
        }

        public async Task UpdateGroupHistory()
        {
            await SqlDapper.ExecuteAsync(_QueryUpdateGroupHistory, commandTimeout: _TimeOut);
        }

        public async Task ClearTempTable() =>
            await SqlDapper.ExecuteAsync("TRUNCATE TABLE GroupHistoryTmp", commandTimeout: _TimeOut);

        private async Task SaveExecute(IGroup[] groups)
        {
            DataTable dt = BuildDataSet(groups);

            using (var connection = await _DataBaseAccess.GetAsync())
            {
                using (var bulkCopy = new SqlBulkCopy(connection))
                {
                    MappingBulkCopyCollumns(bulkCopy);

                    bulkCopy.DestinationTableName = "dbo.GroupHistoryTmp";
                    bulkCopy.BulkCopyTimeout = _TimeOut;

                    await bulkCopy.WriteToServerAsync(dt);
                }

                connection.Close();
                dt.Clear();
            }
        }

        private void MappingBulkCopyCollumns(SqlBulkCopy bulkCopy)
        {
            foreach (var item in _Columns)
            {
                bulkCopy.ColumnMappings.Add(item, item);
            }
        }

        private DataTable BuildDataSet(IGroup[] groups)
        {
            var dataSet = new List<GroupHistoryDTO>();

            foreach (var group in groups)
            {
                foreach (var item in group.Histories)
                {
                    dataSet.Add(new GroupHistoryDTO(group, item));
                }
            }

            return dataSet.ToDataTable(_Columns);
        }

        private IEnumerable<IGroup> Query(string query, object param = null)
        {
            var result = SqlDapper.Query<GroupHistory>(query, param, commandTimeout: _TimeOut);
            return Transform(result);
        }

        private IEnumerable<IGroup> Transform(IEnumerable<GroupHistory> histories)
        {
            foreach (var levelGroup in histories.GroupBy(h => h.RegressionLevel))
            {
                var groups = levelGroup.GroupBy(h => h.Name);

                foreach (var item in groups)
                {
                    var parentName = item.FirstOrDefault()?.ParentName;
                    var historiesOfitem = item.ToList<IGroupHistory>();

                    yield return new Group(levelGroup.Key, item.Key, parentName, historiesOfitem);
                }
            }
        }
    }
}
