using BasicTaskManagement.API.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
            .Build();

string connectionString = config.GetConnectionString("Project") ?? "Error retrieving connection string!";

builder.Services.AddDbContext<BasicTaskManagementContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
