using Domain.Abstraction.Model.Entitys;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using System.Collections.Generic;

namespace Domain.Abstraction.Model
{
    public class TrainedInfosModel
    {
        public TrainedInfosModel()
        {
            RegressionModel = null;
            ProductLocations = null;
        }

        public TrainedInfosModel(IRegressionModel regressionModel, IEnumerable<IProductLocation> productLocations)
        {
            RegressionModel = regressionModel;
            ProductLocations = productLocations;
        }

        public readonly IRegressionModel RegressionModel;
        public readonly IEnumerable<IProductLocation> ProductLocations;
    }
}
