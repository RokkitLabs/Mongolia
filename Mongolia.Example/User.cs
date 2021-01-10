using MongoDB.Bson.Serialization.Attributes;

namespace Mongolia.Example
{
	public class User : Entity
	{
		[BsonElement("username")]
		public string Username { get; set; }
	}
}