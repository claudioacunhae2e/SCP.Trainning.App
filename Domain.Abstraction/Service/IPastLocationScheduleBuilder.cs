using Domain.Abstraction.Model;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using System.Collections.Generic;

namespace Domain.Abstraction.Service
{
    public interface IPastLocationScheduleBuilder
    {
        ILocationHistoryOpen Build();
        IDictionary<string, ILocationHistoryOpen> BuildLevel(IRegressionLevel level);
    }
}
