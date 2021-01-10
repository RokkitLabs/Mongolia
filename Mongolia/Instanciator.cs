using System;
using System.Reflection;
using MongoDB.Bson.Serialization.Attributes;

namespace Mongolia
{

	internal static class Instantiator
	{
		internal static PropertyInfo? GetProperty<T>(string name) {
			Type t = typeof(T);
			PropertyInfo[] properties = t.GetProperties();

			foreach (PropertyInfo propertyInfo in properties)
			{
				BsonElementAttribute attribute = propertyInfo.GetCustomAttribute<BsonElementAttribute>();
				
				if(attribute == null) continue;

				if (attribute.ElementName == name) return propertyInfo;
			}
			
			return t.GetProperty(name);
		}

		internal static T ToType<T>(object obj)
		{
			if (obj is T converted) return converted;

			T tmp = Activator.CreateInstance<T>();
			if (obj == null) return tmp;

			Type objType = obj.GetType();

			foreach (PropertyInfo propertyInfo in objType.GetProperties())
			{
				try
				{
					var prop = GetProperty<T>(propertyInfo.Name);
					if (prop == null) continue;

					prop.SetValue(tmp, propertyInfo.GetValue(obj, null));
				}
				catch { }
			}

			return tmp;
		}
	}
}
