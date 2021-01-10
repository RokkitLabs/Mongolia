using System;

namespace Mongolia
{
	[AttributeUsage(AttributeTargets.Class)]
	public class CollectionNameAttribute: Attribute
	{
		public string Name { get; }
		
		public CollectionNameAttribute(string name)
		{
			Name = name;
		}
	}
}