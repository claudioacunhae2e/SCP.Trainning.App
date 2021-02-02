using Domain.Abstraction.Model;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using System.Collections.Generic;

namespace Domain.Abstraction.Factory
{
    public interface IExperimentFactoryBuilder
    {
        IFactoryInputFilter Input(IRegressionLevel regressionLevel, IRegressionModelInput inputs);

        /// <summary>
        /// Allows to input directly in the format necessary for the experiment
        /// </summary>
        /// <param name="groups">List of inputs</param>
        /// <returns>A wizard for configuring initializers</returns>
        IFactoryInputFilter Input(IRegressionLevel regressionLevel, IEnumerable<IRegressionModelInput> inputs);
    }
}
