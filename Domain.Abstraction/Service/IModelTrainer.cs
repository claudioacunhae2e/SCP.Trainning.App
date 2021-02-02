using Domain.Abstraction.Model;
using Domain.Abstraction.Model.Entitys;

namespace Domain.Abstraction.Service
{
    public interface IModelTrainer
    {
        ModelTrainerAux Start(ISystemInfo SystemInfo);
        TrainedInfosModel Train(IParentName name, ModelTrainerAux aux, bool normalizeProcessCheck);
    }
}
