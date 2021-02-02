using System.Collections.Generic;

namespace Domain.Abstraction.Model
{
    public interface IRegressionModelInput
	{
		string Name { get; }
		string ParentName { get; }
		IList<IInput> Inputs { get; }
	}
}
