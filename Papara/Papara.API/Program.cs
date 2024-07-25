using Autofac.Extensions.DependencyInjection;
using Autofac;
using FluentValidation.AspNetCore;
using Papara.API.Middleware;
using Papara.Business.Validations;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text.Json.Serialization;
using Papara.API.Modules;
using Microsoft.AspNetCore.Mvc;
using Papara.API.Filters;

namespace Papara.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute()))
				.AddFluentValidation(fv => {
					fv.RegisterValidatorsFromAssemblyContaining<CustomerRequestValidator>();
					fv.DisableDataAnnotationsValidation = true; // Veri anotasyonlarýný devre dýþý býrak
				})
				.AddJsonOptions(options =>
				{
					options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
					options.JsonSerializerOptions.WriteIndented = true;
					options.JsonSerializerOptions.PropertyNamingPolicy = null;
				});

			builder.Services.Configure<ApiBehaviorOptions>(options =>
			{
				options.SuppressModelStateInvalidFilter = true;
			});

			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			
			builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
			builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
			{
				containerBuilder.RegisterModule(new AutofacModule(builder.Configuration));
			});

			var app = builder.Build();

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI(opt =>
				{
					opt.DocExpansion(DocExpansion.None);
				});
			}

			app.UseMiddleware<ErrorHandlerMiddleware>();
			app.UseMiddleware<LoggerMiddleware>();
			app.UseMiddleware<HeartbeatMiddleware>();

			app.UseHttpsRedirection();
			app.UseAuthorization();

			app.MapControllers();

			app.Run();

		}
	}
}
