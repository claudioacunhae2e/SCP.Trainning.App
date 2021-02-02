using Domain.Abstraction.Model;
using Domain.Abstraction.Model.Entitys;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Domain.Model
{
    [DataContract]
    [KnownType("GetKnownTypes")]
    public class LocationHistoryOpen : ILocationHistoryOpen
    {
        private readonly ConcurrentDictionary<long, HashSet<long>> _Schedule = new ConcurrentDictionary<long, HashSet<long>>();
        private readonly IDictionary<long, int> _ScheduleByDates = new Dictionary<long, int>();

        public void Add(DateTime date, ILocation location) =>
            Add(date, location.ID);

        public void Add(DateTime date, long locationID)
        {
            HashSet<long> dates;
            var ticks = date.Ticks;

            if (!_Schedule.TryGetValue(locationID, out dates))
            {
                dates = new HashSet<long>();
                _Schedule.TryAdd(locationID, dates);
            }

            dates.Add(ticks);

            if (!_ScheduleByDates.ContainsKey(ticks))
            {
                _ScheduleByDates.Add(ticks, 0);
            }

            _ScheduleByDates[ticks] += 1;
        }

        public bool IsOpen(DateTime date, long location)
        {
            HashSet<long> dates;
            return _Schedule.TryGetValue(location, out dates) && dates.Contains(date.Ticks);
        }

        public int OpenLocationAmount(DateTime date)
        {
            _ScheduleByDates.TryGetValue(date.Ticks, out var storeCount);
            return storeCount;
        }

        public HashSet<long> OpenDatesByLocation(long location)
        {
            HashSet<long> dates;
            _Schedule.TryGetValue(location, out dates);

            return dates;
        }
    }
}
