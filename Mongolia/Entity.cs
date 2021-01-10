using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Mongolia
{
	public class Entity
	{
		protected internal DB db;

		[BsonId]
		public ObjectId ID { get; set; }
	}
}