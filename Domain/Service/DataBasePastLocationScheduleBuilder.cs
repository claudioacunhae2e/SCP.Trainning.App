using Dapper;
using Domain.Abstraction.Model;
using Domain.Abstraction.Model.Entitys;
using Domain.Abstraction.Service;
using Domain.Model;
using Domain.Repository;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using Old._42.SCP.Domain.Abstractions.Bases.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Service
{
    public class DataBasePastLocationScheduleBuilder : IPastLocationScheduleBuilder
    {
        public DataBasePastLocationScheduleBuilder(
            ILocationRepository locations,
            IDataBaseAccess dataBaseAccess)
        {
            _Locations = locations;
            _DataBaseAccess = dataBaseAccess;
        }

        private const string _QueryBase = @"
			WITH LocationDailySales AS (
			  SELECT 
					 ProductLocationHistory.Location Location, 
					 ProductLocationHistory.Date,
					 SUM(ProductLocationHistory.SalesRevenue) Revenue
				FROM ProductLocationHistory
				JOIN Location ON Location.ID = ProductLocationHistory.Location
			GROUP BY ProductLocationHistory.Location, ProductLocationHistory.Date
			HAVING SUM(ProductLocationHistory.SalesRevenue) > 0
			),
			LocationTolerance AS (
			  SELECT 
					 LocationDailySales.Location Location, 
					 (AVG(LocationDailySales.Revenue) * 0.1) Tolerance
				FROM LocationDailySales
			GROUP BY LocationDailySales.Location
			)
			SELECT 
				  LocationDailySales.Location Location, 
				  LocationDailySales.Date Date
			 FROM LocationDailySales
			 JOIN LocationTolerance ON LocationTolerance.Location = LocationDailySales.Location
			 WHERE LocationDailySales.Revenue > LocationTolerance.Tolerance
			 ORDER BY LocationDailySales.Date    ";

        private readonly ILocationRepository _Locations;
        private readonly IDataBaseAccess _DataBaseAccess;
        private readonly Func<long, DateTime, (long Location, DateTime Date)> _Map = (long Location, DateTime Date) => (Location, Date);

        public ILocationHistoryOpen Build()
        {
            var schedule = new LocationHistoryOpen();

            var data = GetSchedules();
            var locations = _Locations.AllAsDictionary();

            foreach (var (location, date) in data)
            {
                schedule.Add(date, locations[location]);
            }

            return schedule;
        }

        public IDictionary<string, ILocationHistoryOpen> BuildLevel(IRegressionLevel level)
        {
            var data = GetSchedules();
            var locations = _Locations.AllAsDictionary();

            return BuildByLevel(level, data, locations);
        }

        private IDictionary<string, ILocationHistoryOpen> BuildByLevel(IRegressionLevel level, IEnumerable<(long Location, DateTime Date)> data, IDictionary<long, ILocation> locations)
        {
            var result = new Dictionary<string, ILocationHistoryOpen>();
            var dateGroups = data.GroupBy(c => locations[c.Location].GetGroupName(level.LocationGroupers));

            foreach (var item in dateGroups)
            {
                var schedule = new LocationHistoryOpen();

                foreach (var group in item)
                {
                    schedule.Add(group.Date, locations[group.Location]);
                }

                if (!result.ContainsKey(item.Key))
                {
                    result.Add(item.Key, schedule);
                }
            }

            return result;
        }

        private IEnumerable<(long Location, DateTime Date)> GetSchedules()
        {
            IEnumerable<(long Location, DateTime Date)> result;

            using (var conn = _DataBaseAccess.Get())
            {
                result = conn.Query(_QueryBase, _Map, splitOn: "Location,Date", commandTimeout: 0);
            }

            return result;
        }
    }
}
