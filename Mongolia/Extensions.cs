using MongoDB.Bson;
using System;
using System.Threading.Tasks;

namespace Mongolia
{
	public static class Extensions
	{
		private static void checkHasDB<T>(T doc) where T : Entity
		{
			if (doc.db == null)
				throw new NullReferenceException("Entity does not have a repository attached to it. Was it created using a repository?");
		}
		
		public static Task<ObjectId> Save<T>(this T doc) where T: Entity
		{
			checkHasDB(doc);
			return doc.db.GetRepository<T>().Save(doc);
		}
		
		public static Task<bool> Delete<T>(this T doc) where T: Entity
		{
			checkHasDB(doc);
			return doc.db.GetRepository<T>().DeleteOne(doc);
		}
	}
}
