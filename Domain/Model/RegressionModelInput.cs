using Domain.Abstraction.Model;
using System.Collections.Generic;

namespace Domain.Model
{
    public class RegressionModelInput : IRegressionModelInput
	{
		public string Name { get; }
		public string ParentName { get; }
		public long? ItemID { get; }
		public IList<IInput> Inputs { get; }
		public RegressionModelInput(string name, string parentName, long? itemID, IList<IInput> inputs)
		{
			Name = name;
			ParentName = parentName;
			ItemID = itemID;
			Inputs = inputs;
		}
	}
}
