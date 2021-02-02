using Domain.Abstraction.Model;
using E2E.Generic.Helpful.ThreadSafe;

namespace Domain.Model
{
    public class TrainingAppServiceModel
    {
        public TrainingAppServiceModel(int itensCounter, IParentName item, ModelTrainerAux modelTrainer, StopwatchThreadSafe watch)
        {
            ItensCounter = itensCounter;
            Item = item;
            ModelTrainer = modelTrainer;
            Watch = watch;
        }

        public readonly int ItensCounter;
        public readonly IParentName Item;
        public readonly ModelTrainerAux ModelTrainer;
        public readonly StopwatchThreadSafe Watch;
    }
}
