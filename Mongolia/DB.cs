using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace Mongolia
{
	public class DB
	{
		public MongoClient Client { get; }
		public IMongoDatabase Db { get; }

		private Dictionary<Type, object> repositories;

		/// <summary>
		/// Creates a new Mongolia db instance
		/// </summary>
		/// <param name="client">The client to use</param>
		/// <param name="databaseName">Name of the database</param>
	    public DB(MongoClient client, string databaseName)
		{
			Client = client;
			Db = client.GetDatabase(databaseName);
			repositories = new Dictionary<Type, object>();
		}

		/// <summary>
		/// Creates a new Mongolia db instance
		/// </summary>
		/// <param name="clientSettings">Connection settings to use</param>
		/// <param name="databaseName">Name of the database</param>
		public DB(MongoClientSettings clientSettings, string databaseName): this(new MongoClient(clientSettings), databaseName)
		{
		}

		/// <summary>
		/// Creates a new Mongolia db instance
		/// </summary>
		/// <param name="uri">Mongo uri in format mongodb://localhost:27017/</param>
		/// <param name="databaseName">Name of the database</param>
		public DB(string uri, string databaseName): this(MongoClientSettings.FromUrl(new MongoUrl(uri)), databaseName)
		{
		}

		/// <summary>
		/// Gets a database repository for an entity
		/// </summary>
		/// <typeparam name="T">Type of the entity</typeparam>
		/// <returns>The repository</returns>
		public DBRepository<T> GetRepository<T>() where T: Entity
		{
			if (!repositories.ContainsKey(typeof(T)))
				repositories[typeof(T)] = new DBRepository<T>(this);

			return (DBRepository<T>) repositories[typeof(T)];

		}
	}
}