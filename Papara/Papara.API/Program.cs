
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

			// Add services to the container.

			builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute()))
				.AddFluentValidation(fv => { fv.RegisterValidatorsFromAssemblyContaining<CustomerRequestValidator>(); fv.DisableDataAnnotationsValidation = true; } )
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
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();


			builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()); // Bu satýr, uygulamanýn varsayýlan hizmet saðlayýcý fabrikasýný Autofac ile deðiþtirmek için kullanýlýr.
			builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
			{
				containerBuilder.RegisterModule(new AutofacModule(builder.Configuration));
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
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
