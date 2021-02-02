using Domain.Abstraction.Model.Entitys;
using Old._42.SCP.Domain.Abstractions.Bases.Entitys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Abstraction.Repository
{
    public interface IEventDateFeatureRepository : IRepository<IEventDateFeature>
	{
		Task<IEnumerable<IEventDateFeature>> By(DateTime start, DateTime end, string scope = "");
		Task<IList<IEventDateFeature>> ByComponetized(DateTime start, DateTime end, string scope = "");
	}
}
