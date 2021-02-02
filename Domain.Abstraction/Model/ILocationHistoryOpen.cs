using System;
using System.Collections.Generic;

namespace Domain.Abstraction.Model
{
    public interface ILocationHistoryOpen
    {
        bool IsOpen(DateTime date, long location);
        int OpenLocationAmount(DateTime date);
        HashSet<long> OpenDatesByLocation(long location);
    }
}
