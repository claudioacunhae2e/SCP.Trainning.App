using Domain.Abstraction.Model;
using System.Threading.Tasks;

namespace Domain.Abstraction.Service
{
    public interface IProcessService
	{
		Task Init(IProcessServiceDecisions decisions);
	}
}
