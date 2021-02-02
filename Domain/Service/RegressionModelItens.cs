using Domain.Abstraction.Model;
using Domain.Abstraction.Repository;
using Domain.Abstraction.Service;
using Domain.Model;
using E2E.Infra.SQL;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using Old._42.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Service
{
    public class RegressionModelItens : IRegressionModelItens
	{
		public RegressionModelItens(
			IRegressionLevelRepository regressionLevels,
			ISystemInfoRepository systemInfos)
		{
			_RegressionLevels = regressionLevels;
			_SystemInfos = systemInfos;
		}

		private readonly IRegressionLevelRepository _RegressionLevels;
		private readonly ISystemInfoRepository _SystemInfos;

		private const string _QueryDefault = @"
				SELECT DISTINCT
						{0} item,
						{1} parentName,
						count(Distinct ProductLocation.ID)  ProductLocationCount
				 FROM ProductLocationHistory 
				 JOIN ProductLocation ON ProductLocation.ID = ProductLocationHistory.ProductLocation
				 JOIN Product		  ON Product.ID = ProductLocation.Product
				 JOIN Location		  ON Location.ID = ProductLocation.Location
				 WHERE {0} IS NOT NULL
						AND Location.Type = 2 
                       {2}  
				GROUP BY {0}, {1}";

		private const string _QueryRegressionModelsToTrain = @"
				SELECT DISTINCT
						{0} Name,
						{1} parentName
				 FROM ProductLocation 
				 JOIN Product		  ON Product.ID = ProductLocation.Product
				 JOIN Location		  ON Location.ID = ProductLocation.Location
				 WHERE {0} IS NOT NULL 
						AND (ProductLocation.QuantityOnHand > 0)
				GROUP BY {0}, {1}";

		public async Task<IEnumerable<IGroupNameDTO>> By(IRegressionLevel regressionLevel, bool shouldCalculateIncremental, DateTime stabilityDate)
		{
			var query = GetGroupsQuery(regressionLevel, shouldCalculateIncremental);
			var param = new
			{
				stabilityDate
			};

			var result = await SqlDapper.QueryAsync<(string item, string parentName, int productLocationsCount)>(query, param, commandTimeout: 0);

			return result.Select(x => new GroupNameDTO(x.item, x.parentName, x.productLocationsCount));
		}

		protected string GetFilterFormat(IRegressionLevel regressionLevel)
		{
			var parts = new List<string>();

			if (regressionLevel.LocationGroupers.NotEmpty())
			{
				parts.AddRange(regressionLevel.LocationGroupers.Select(l => string.Concat("Location.[", l, "]")));
			}

			if (regressionLevel.ProductGroupers.NotEmpty())
			{
				parts.AddRange(regressionLevel.ProductGroupers.Select(p => string.Concat("Product.[", p, "]")));
			}

			return parts.IsNotNull() ? string.Join("+'|'+", parts) : "''";
		}

		protected virtual string GetGroupsQuery(IRegressionLevel regressionLevel, bool shouldCalculateIncremental)
		{
			var regressionModelFilter = GetFilterFormat(regressionLevel);

			var parentRegressionModelFilter = regressionLevel.AscendantID.IsNotNull()
							   ? GetFilterFormat(_RegressionLevels.ByID(regressionLevel.AscendantID.Value))
							   : "''";

			var CalculateIncrementalFilter = shouldCalculateIncremental
												? " AND (ProductLocation.QuantityOnHand > 0 OR ProductLocationHistory.Date > @stabilityDate )"
												: " ";

			// //Trecho abaixo para filtrar um item específico para debug
			var query = string.Format(_QueryDefault, regressionModelFilter, parentRegressionModelFilter, CalculateIncrementalFilter);

			return query;
		}


		public async Task<IEnumerable<IParentName>> GetRegressionModelsToTrain(IRegressionLevel regressionLevel)
		{
			var regressionModelFilter = GetFilterFormat(regressionLevel);

			var parentRegressionModelFilter = regressionLevel.AscendantID.IsNotNull()
							   ? GetFilterFormat(_RegressionLevels.ByID(regressionLevel.AscendantID.Value))
							   : regressionModelFilter;

			var query = string.Format(_QueryRegressionModelsToTrain, regressionModelFilter, parentRegressionModelFilter);

			var result = await SqlDapper.QueryAsync<ParentNameModel>(query, commandTimeout: 0);

			return result;
		}
	}
}
