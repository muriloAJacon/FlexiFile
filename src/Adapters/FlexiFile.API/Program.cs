using Autofac.Extensions.DependencyInjection;
using FlexiFile.API.Configurations;
using FlexiFile.API.Filters;
using FlexiFile.API.Options;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using MediatR;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

Log.Logger = new LoggerConfiguration()
					.ReadFrom.Configuration(builder.Configuration)
					.CreateBootstrapLogger();

builder.Host.UseSerilog();

builder.Services.AddBearerAuthentication(builder.Configuration);

builder.Services.ConfigureCors(builder.Configuration);

builder.Services.AddControllers(ExtensionOptions.ConfigureControllers)
				.AddJsonOptions(ExtensionOptions.ConfigureJson);

builder.Services.AddSwaggerGen();

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

builder.Services.AddApiVersioning(ExtensionOptions.ConfigureApiVersioning);

builder.Services.AddVersionedApiExplorer(ExtensionOptions.ConfigureApiVersioningExplorer);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMassTransit(x => ExtensionOptions.ConfigureMassTransit(x, builder.Configuration));

builder.Services.AddHttpContextAccessor();

builder.Services.AddPostgres(builder.Configuration, builder.Environment);

builder.Services.AddRepositories();

builder.Services.AddValidatorsFromAssembly(AppDomain.CurrentDomain.Load("FlexiFile.Application"));

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.AddFluentValidationRulesToSwagger();

builder.Services.AddMediatR(ExtensionOptions.ConfigureMediatR)
				.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviorFilter<,>));

builder.Services.AddDependencyInjection();


var app = builder.Build();

app.UseMigrations();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI(opts => ExtensionOptions.ConfigureSwaggerUI(opts, app));
}

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
