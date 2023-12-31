﻿using FlexiFile.Core.OptionsBuilder;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using RabbitMQ.Client;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text.Json.Serialization;

namespace FlexiFile.API.Options {
	public static class ExtensionOptions {
		public static void ConfigureMediatR(MediatRServiceConfiguration options) {
			options.RegisterServicesFromAssemblyContaining<Program>();
			options.RegisterServicesFromAssembly(AppDomain.CurrentDomain.Load("FlexiFile.Application"));
		}

		public static void ConfigureSwaggerUI(SwaggerUIOptions options, WebApplication app) {
			var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

			foreach (var groupName in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse().Select(x => x.GroupName)) {
				options.SwaggerEndpoint($"/swagger/{groupName}/swagger.json",
					groupName.ToUpperInvariant());
			}
		}

		public static void ConfigureMassTransit(IBusRegistrationConfigurator configurator, IConfiguration configuration) {
			configurator.UsingRabbitMq((ctx, cfg) => {
				cfg.Host(configuration.GetConnectionString("RabbitMQ"));

				cfg.ReceiveEndpoint("FlexiFile.API", e => {
					e.ExchangeType = ExchangeType.Topic;
				});
			});
		}

		public static void ConfigureControllers(MvcOptions options) {
			options.Filters.Add(new ProducesAttribute("application/json"));
		}

		public static void ConfigureJson(JsonOptions options) {
			options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
			options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
		}

		public static void ConfigureApiVersioning(ApiVersioningOptions options) {
			options.DefaultApiVersion = new ApiVersion(1, 0);
			options.AssumeDefaultVersionWhenUnspecified = true;
			options.ReportApiVersions = true;
			options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
															new HeaderApiVersionReader("x-api-version"),
															new MediaTypeApiVersionReader("x-api-version"));
		}

		public static void ConfigureApiVersioningExplorer(ApiExplorerOptions options) {
			options.GroupNameFormat = "'v'VVV";
			options.SubstituteApiVersionInUrl = true;
		}

		public static void ConfigureWorkerHub(SignalRClientOptionsBuilder options, IConfiguration configuration) {
			options.WithConnectionString(configuration.GetConnectionString("WorkerHub") ?? throw new Exception("Worker hub connection string not found"));
		}
	}
}
