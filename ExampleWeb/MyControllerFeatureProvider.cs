using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Mongolia.ExampleWeb
{
	public class MyControllerFeatureProvider: IApplicationFeatureProvider<ControllerFeature>
	{
		public Type[] Controllers;

		public MyControllerFeatureProvider(params Type[] controllers)
		{
			Controllers = controllers;
		}
		
		public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
		{
			foreach (Type controllerType in Controllers)
			{
				feature.Controllers.Add(controllerType.GetTypeInfo());
			}
		}
	}
}