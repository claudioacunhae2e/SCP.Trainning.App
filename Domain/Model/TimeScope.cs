using Domain.Abstraction.Model;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Domain.Model
{
    [DataContract]
    [KnownType("GetKnownTypes")]

    public class TimeScope : ITimeScope
    {
        public TimeScope(DateTime start, DateTime end)
        {
            Start = start.Date;
            End = end.Date;
            CalculatedDaysBackward = new List<DateTime>();
            CalculatedDaysForward = new List<DateTime>();

            AddCalculatedDaysBackward();
            AddCalculatedDaysForward();
        }

        private readonly List<DateTime> CalculatedDaysBackward;
        private readonly List<DateTime> CalculatedDaysForward;

        [DataMember]
        public DateTime Start { get; }

        [DataMember]
        public DateTime End { get; }

        public bool IsValid() =>
            End > Start;

        public IEnumerable<DateTime> DaysBackward() =>
            CalculatedDaysBackward;

        public IEnumerable<DateTime> DaysForward() =>
            CalculatedDaysForward;

        public bool IsIn(DateTime date) =>
            Start <= date && End >= date;

        public override string ToString() =>
            string.Concat("Time Scope Start : ", Start, " , End: ", End);

        private void AddCalculatedDaysForward()
        {
            for (var dt = Start; dt <= End; dt = dt.AddDays(1))
            {
                CalculatedDaysForward.Add(dt);
            }
        }

        private void AddCalculatedDaysBackward()
        {
            for (var dt = End.Date; dt >= Start.Date; dt = dt.AddDays(-1))
            {
                CalculatedDaysBackward.Add(dt);
            }
        }
    }
}
