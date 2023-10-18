using Autofac.Extensions.DependencyInjection;
using FlexiFile.Application.Hubs;
using FlexiFile.Worker.Configurations;
using FlexiFile.Worker.Filters;
using FlexiFile.Worker.Options;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddPostgres(builder.Configuration, builder.Environment);

builder.Services.AddRepositories();

builder.Services.AddMediatR(ExtensionOptions.ConfigureMediatR)
				.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviorFilter<,>));


builder.Services.AddSignalR().AddJsonProtocol(ExtensionOptions.ConfigureJson);

builder.Services.AddDependencyInjection();

builder.Services.ConfigureOptions(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapHub<ConvertHub>("/convert");

app.Run();