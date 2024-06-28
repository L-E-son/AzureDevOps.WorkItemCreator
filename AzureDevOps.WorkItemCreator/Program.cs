using AzureDevOps.WorkItemCreator;
using AzureDevOps.WorkItemCreator.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.Services.Common;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", false, true);

builder.Services.AddOptions();
builder.Services.Configure<DevOpsProject>(builder.Configuration.GetSection("DevOpsProject"));

builder.Services.AddScoped<WorkItemCreator>();

using IHost host = builder.Build();

var create = host.Services.GetRequiredService<WorkItemCreator>();

var userStories = UserStoryGenerator.CreateUserStories();
await create.CreateUserStory(userStories);

// Lifetime services...

await host.RunAsync();