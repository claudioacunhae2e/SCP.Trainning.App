using Domain.Abstraction.Model;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using System.Collections.Generic;

namespace Domain.Abstraction.Factory
{
    public interface IFactoryConfigured
    {
        IEnumerable<IRegressionModel> Train();
        IRegressionModel Train(IRegressionModelInput input);
    }
}
