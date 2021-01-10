using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mongolia
{
	public class DBRepository<T> where T: Entity
	{
		private readonly DB db;
		private readonly IMongoCollection<T> collection;
		
		public DBRepository(DB db)
		{
			this.db = db;
			
			Type t = typeof(T);
			CollectionNameAttribute attribute = t.GetCustomAttribute<CollectionNameAttribute>();
			
			collection = db.Db.GetCollection<T>(attribute == null ? t.Name : attribute.Name);
		}

		/// <summary>
		/// Saves an entity to the database
		/// </summary>
		/// <param name="doc"></param>
		/// <returns></returns>
		public async Task<ObjectId> Save(T doc)
		{
			if (doc.ID == default || doc.ID == ObjectId.Empty)
				await collection.InsertOneAsync(doc);
			else
				await collection.ReplaceOneAsync(e => e.ID == doc.ID, doc);

			return doc.ID;
		}

		/// <summary>
		/// Creates type of entity and sets database
		/// </summary>
		/// <param name="doc"></param>
		/// <returns></returns>
		public async Task<T> Create(object obj)
		{
			T created = Instantiator.ToType<T>(obj);
			created.db = db;
			await created.Save();
			
			return created;
		}

		/// <summary>
		/// Finds one document by object id
		/// </summary>
		/// <param name="id">Id of the document</param>
		/// <returns>The document</returns>
		public Task<T> FindOne(ObjectId id)
		{
			return FindOne(e => e.ID == id);
		}

		/// <summary>
		/// Finds a document by an object
		/// </summary>
		/// <example>
		/// await userRepo.FindOne(new { Username = "example user" });
		/// await userRepo.FindOne(new User() { Username = "example user" });
		/// </example>
		/// <param name="doc"></param>
		/// <returns></returns>
		public Task<T> FindOne(object doc)
		{
			FilterDefinition<T> filter = BuildFilterFromObj(doc);
			return FindOne(filter);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filter"></param>
		/// <returns></returns>
		public async Task<T> FindOne(FilterDefinition<T> filter)
		{
			IAsyncCursor<T> results = await collection.FindAsync(filter);
			T result = await results.FirstOrDefaultAsync();

			result.db = db;
			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		public Task<T> FindOne(Expression<Func<T, bool>> expression)
		{
			FilterDefinition<T> filter = new ExpressionFilterDefinition<T>(expression);
			return FindOne(filter);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public FilterDefinition<T> BuildFilterFromObj(object obj)
		{
			List<FilterDefinition<T>> filters = new List<FilterDefinition<T>>();

			foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
			{
				try
				{
					PropertyInfo prop = Instantiator.GetProperty<T>(propertyInfo.Name);

					object value = propertyInfo.GetValue(obj, null);
					filters.Add(Builders<T>.Filter.Eq(prop?.Name ?? propertyInfo.Name, value));
				}
				catch
				{
					// If it fails to get the property or fails to set the property, it defaults to the name on the object 
				}
			}

			return Builders<T>.Filter.And(filters.ToArray());
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Task<IList<T>> Find(ObjectId id)
		{
			return Find(e => e.ID == id);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="doc"></param>
		/// <returns></returns>
		public Task<IList<T>> Find(object doc)
		{
			FilterDefinition<T> filter = BuildFilterFromObj(doc);
			return Find(filter);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Task<IList<T>> FindAll()
		{
			return Find(_ => true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filter"></param>
		/// <returns></returns>
		public async Task<IList<T>> Find(FilterDefinition<T> filter)
		{
			IAsyncCursor<T> cursor = await collection.FindAsync(filter);
			List<T> results = await cursor.ToListAsync();
			
			foreach (T result in results)
			{
				result.db = db;
			}

			return results;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		public Task<IList<T>> Find(Expression<Func<T, bool>> expression)
		{
			FilterDefinition<T> filter = new ExpressionFilterDefinition<T>(expression);
			return Find(filter);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="doc"></param>
		/// <returns></returns>
		/// <exception cref="NullReferenceException"></exception>
		public async Task<bool> DeleteOne(T doc)
		{
			if (doc.ID == default || doc.ID == ObjectId.Empty)
				throw new NullReferenceException("Document does not have ID");

			DeleteResult result = await collection.DeleteOneAsync(e => e.ID == doc.ID);
			return result.IsAcknowledged;
		}
	}
}