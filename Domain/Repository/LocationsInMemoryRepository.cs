using Domain.Abstraction.Model.Entitys;
using Domain.Model.Entitys;
using Domain.Standard.Enums;
using Old._42.SCP.Domain.Abstractions.Bases.Entitys;
using Old._42.SCP.Domain.Bases.Entitys;
using Old._42.Util.Extensions;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Domain.Repository
{
	public class LocationsInMemoryRepository : BaseInMemoryRepository<ILocation>, ILocationRepository
	{
		public readonly IList<string> Properties = new List<string> { "ID" };

		public LocationsInMemoryRepository(IDataBaseAccess dataBaseAccess)
			: base("Location", dataBaseAccess)
		{
		}


		public ILocation By(string clientID)
		{
			throw new System.NotImplementedException();
		}

		public IEnumerable<ILocation> By(IEnumerable<string> locations)
		{
			throw new System.NotImplementedException();
		}

		public IDictionary<long, ILocation> ToSupplyAsDictionary(IEnumerable<long> ids)
		{
			var entities = new Dictionary<long, ILocation>();

			var arbitrarySize = 2000;
			foreach (var part in ids.BreakEach(arbitrarySize))
			{
				var query = $"SELECT * FROM Location WHERE [ID] IN ({part.Select(x => x.ToString()).Join(",")})";
				entities.AddRange(ListAsDictionary(query));
			}

			return entities;
		}

		protected override ILocation BuildEntity(DataRow row, IEnumerable<DataColumn> columns)
		{
			var attributes = row.ToDictionary(columns.Where(x => !Properties.Contains(x.ColumnName)));
			var id = row.Get<long>("ID");
			var type = (LocationType)row.Get<int>("Type");
			var cdClientID = row.Get<string>("CDClientID");
			var ClientID = row.Get<string>("ClientID");
			return new Location(id, attributes, attributes["Description"], type, cdClientID, ClientID);
		}
	}
}
