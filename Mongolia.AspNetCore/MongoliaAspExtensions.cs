using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Mongolia.AspNetCore
{
	public static class MongoliaAspExtensions
	{
		public static void AddDB(this IServiceCollection services, DB db, params Type[] entities)
		{
			services.TryAddSingleton(db);
			
			foreach (Type entityType in entities)
			{
				object repo = db.GetType().GetMethod("GetRepository")?.MakeGenericMethod(entityType).Invoke(db, null);
				services.TryAdd(new ServiceDescriptor(typeof(DBRepository<>).MakeGenericType(entityType), repo));
			}
		}
	}
}