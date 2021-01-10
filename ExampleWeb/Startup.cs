using Mongolia.ExampleWeb.Controllers;
using Mongolia.ExampleWeb.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Mongolia.AspNetCore;

namespace Mongolia.ExampleWeb
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			DB db = new DB("mongodb://localhost:27017/?ssl=false", "mongoliaTesting");
			services.AddDB(db, typeof(Note));
			
			ApplicationPartManager partManager = new ApplicationPartManager();
			partManager.FeatureProviders.Clear();
			partManager.FeatureProviders.Add(new MyControllerFeatureProvider(
				typeof(NoteController)
			));
			
			services.TryAddSingleton(partManager);
			
			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo {Title = "ExampleWeb", Version = "v1"});
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
				app.UseDeveloperExceptionPage();

			app.UseSwagger();
			app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ExampleWeb v1"));
			
			app.UseRouting();

			app.UseAuthorization();
			app.UseAuthentication();
			
			app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
		}
	}
}