namespace Domain.Abstraction.Factory
{
    public interface IFactoryModelRefiner
    {
        IFactoryModelEvaluator AddModelRefiner(IRefiner refiner);
    }
}
