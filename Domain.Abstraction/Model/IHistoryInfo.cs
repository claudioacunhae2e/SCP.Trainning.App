namespace Domain.Abstraction.Model
{
    public interface IHistoryInfo : IHistory
	{
		bool HasSales { get; }
		bool HasInventory { get; }
	}
}
