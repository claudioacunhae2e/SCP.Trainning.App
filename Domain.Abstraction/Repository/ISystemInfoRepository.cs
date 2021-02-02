using Domain.Abstraction.Model.Entitys;
using Old._42.SCP.Domain.Abstractions.Bases.Entitys;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstraction.Repository
{
    public interface ISystemInfoRepository : IRepository<ISystemInfo>
    {
        Task<ISystemInfo> Get(string scope = null);
        void UpdateLastTraining();
        void UpdateLastLambdaCalculation();
        void UpdateLastDistribution();
        void UpdateLastNormalizationStabilityLimit(DateTime value);
    }
}
