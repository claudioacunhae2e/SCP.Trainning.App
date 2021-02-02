namespace Domain.Abstraction.Factory
{
    public interface IFactoryInput<T>
	{
		IFactoryInputFilter InputTransformer(IDataToInputTransformer<T> transformer);
		IFactoryInputFilter InputTransformer<G>() where G : IDataToInputTransformer<T>, new();
		IFactoryModelInitializer ObservationTransformer(IDataToObservationTransformer<T> transformer);
		IFactoryModelInitializer ObservationTransformer<G>() where G : IDataToObservationTransformer<T>, new();
	}
}
