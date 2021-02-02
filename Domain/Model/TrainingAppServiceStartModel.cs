using Domain.Abstraction.Model;
using System.Collections.Generic;

namespace Domain.Model
{
    public class TrainingAppServiceStartModel
    {
        public TrainingAppServiceStartModel(IEnumerable<IParentName> itens, ModelTrainerAux modelTrainer)
        {
            Itens = itens;
            ModelTrainer = modelTrainer;
        }

        public readonly IEnumerable<IParentName> Itens;
        public readonly ModelTrainerAux ModelTrainer;
    }
}
