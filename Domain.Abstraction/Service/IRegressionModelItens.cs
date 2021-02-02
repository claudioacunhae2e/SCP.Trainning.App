using Domain.Abstraction.Model;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Abstraction.Service
{
    public interface IRegressionModelItens
    {
        Task<IEnumerable<IGroupNameDTO>> By(IRegressionLevel regressionLevel, bool shouldCalculateIncremental, DateTime stabilityDate);

        Task<IEnumerable<IParentName>> GetRegressionModelsToTrain(IRegressionLevel regressionLevel);
    }
}
