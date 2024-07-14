
using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Papara.API.Middleware;
using Papara.API.ServiceTest;
using Papara.Business.Command.CustomerCommand.Create;
using Papara.Business.Mapping;
using Papara.Business.Validations;
using Papara.Data.Context;
using Papara.Data.UnitOfWork;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Papara.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers()
				.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<CustomerRequestValidator>())
				.AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
				options.JsonSerializerOptions.WriteIndented = true;
				options.JsonSerializerOptions.PropertyNamingPolicy = null;
			}); ;
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			string connectionStringMsSql = builder.Configuration.GetConnectionString("MsSqlConnection");
			builder.Services.AddDbContext<PaparaDbContext>(x => x.UseSqlServer(connectionStringMsSql));


			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

			var config = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(new MapperConfig());
			});

			builder.Services.AddSingleton(config.CreateMapper());

			builder.Services.AddMediatR(typeof(CreateCustomerCommand).GetTypeInfo().Assembly);


			builder.Services.AddTransient<CustomService>();
			builder.Services.AddScoped<CustomService2>();
			builder.Services.AddSingleton<CustomService3>();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}


			app.UseMiddleware<HeartbeatMiddleware>();
			app.UseMiddleware<ErrorHandlerMiddleware>();

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();


			//app.Use((context, next) =>
			//{
			//	if (!string.IsNullOrEmpty(context.Request.Path) && context.Request.Path.Value.Contains("favicon"))
			//	{
			//		return next();
			//	}

			//	var service1 = context.RequestServices.GetRequiredService<CustomService>();
			//	var service2 = context.RequestServices.GetRequiredService<CustomService2>();
			//	var service3 = context.RequestServices.GetRequiredService<CustomService3>();

			//	service1.Counter++;
			//	service2.Counter++;
			//	service3.Counter++;

			//	return next();
			//});

			//app.Run(async context =>
			//{
			//	var service1 = context.RequestServices.GetRequiredService<CustomService>();
			//	var service2 = context.RequestServices.GetRequiredService<CustomService2>();
			//	var service3 = context.RequestServices.GetRequiredService<CustomService3>();

			//	if (!string.IsNullOrEmpty(context.Request.Path) && !context.Request.Path.Value.Contains("favicon"))
			//	{
			//		service1.Counter++;
			//		service2.Counter++;
			//		service3.Counter++;
			//	}

			//	await context.Response.WriteAsync($"Service1 : {service1.Counter}\n");
			//	await context.Response.WriteAsync($"Service2 : {service2.Counter}\n");
			//	await context.Response.WriteAsync($"Service3 : {service3.Counter}\n");
			//});

			app.Run();

		}
	}
}
