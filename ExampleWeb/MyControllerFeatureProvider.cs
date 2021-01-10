using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Mongolia.ExampleWeb
{
	public class MyControllerFeatureProvider: IApplicationFeatureProvider<ControllerFeature>
	{
		private Type[] controllers;

		public MyControllerFeatureProvider(params Type[] controllers)
		{
			this.controllers = controllers;
		}
		
		public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
		{
			foreach (Type controllerType in controllers)
			{
				feature.Controllers.Add(controllerType.GetTypeInfo());
			}
		}
	}
}