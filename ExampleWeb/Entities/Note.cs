using MongoDB.Bson.Serialization.Attributes;
using Mongolia;

namespace Mongolia.ExampleWeb.Entities
{
	public class Note: Entity
	{
		[BsonElement("title")]
		public string Title { get; set; }
		
		[BsonElement("desc")]
		public string Description { get; set; }
	}
}