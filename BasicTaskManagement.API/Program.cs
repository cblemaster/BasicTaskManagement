using BasicTaskManagement.API.Context;
using BasicTaskManagement.Core.DTO;
using BasicTaskManagement.Core.Entities;
using BasicTaskManagement.Core.Mappers;
using BasicTaskManagement.Core.Validation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Context = BasicTaskManagement.API.Context.BasicTaskManagementContext;

var builder = WebApplication.CreateBuilder(args);

IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
            .Build();

string connectionString = config.GetConnectionString("Project") ?? "Error retrieving connection string!";

builder.Services.AddDbContext<BasicTaskManagementContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

app.MapGet("/", () => "Welcome to BasicTaskManagement!");

app.MapGet("/taskgroup", Results<NotFound<string>, Ok<IEnumerable<TaskGroupDTO>>> (Context context) =>
{
    IEnumerable<TaskGroupDTO> groups = EntityToDTO.MapTaskGroupCollection(context.TaskGroups.Include(tg => tg.TaskItems.Where(ti => !ti.IsComplete).OrderByDescending(ti => ti.DueDate)));
    return groups is null || !groups.Any()
        ? TypedResults.NotFound("No task groups found.")
        : TypedResults.Ok(groups.OrderByDescending(g => g.IsFavorite).ThenBy(g => g.Name).AsEnumerable());
});

app.MapGet("/taskgroup/{id:int}", async Task<Results<BadRequest<string>, Ok<TaskGroupDTO>, NotFound<string>>> (Context context, int id) =>
{
    if (id < 1)
    {
        return TypedResults.BadRequest("Invalid task group id.");
    }
    if (await context.TaskGroups.SingleOrDefaultAsync(tg => tg.Id == id) is TaskGroup taskgroup)
    {
        TaskGroupDTO group = EntityToDTO.MapTaskGroup(taskgroup);
        return TypedResults.Ok(group);
    }
    return TypedResults.NotFound($"No task group with id {id} found.");
});

app.MapGet("/taskgroup/showcomplete", Results<NotFound<string>, Ok<IEnumerable<TaskGroupDTO>>> (Context context) =>
{
    IEnumerable<TaskGroupDTO> groups = EntityToDTO.MapTaskGroupCollection(context.TaskGroups.Include(tg => tg.TaskItems.OrderByDescending(ti => ti.DueDate)));
    return groups is null || !groups.Any()
        ? TypedResults.NotFound("No task groups found.")
        : TypedResults.Ok(groups.OrderByDescending(g => g.IsFavorite).ThenBy(g => g.Name).AsEnumerable());
});

app.MapPost("/taskgroup", async Task<Results<BadRequest<string>, Created<TaskGroupDTO>>> (Context context, CreateTaskGroupDTO createGroup) =>
{
    if (createGroup is null)
    {
        return TypedResults.BadRequest("No group to create provided.");
    }

    ValidationResult validationResult = createGroup.Validate();

    if (!validationResult.IsValid)
    {
        return TypedResults.BadRequest(validationResult.ErrorMessage);
    }

    TaskGroup groupToCreate = DTOToEntity.MapCreateTaskGroup(createGroup);
    context.TaskGroups.Add(groupToCreate);
    await context.SaveChangesAsync();

    return TypedResults.Created($"/taskgroup/{groupToCreate.Id}", EntityToDTO.MapTaskGroup(groupToCreate));
});

app.MapDelete("/taskgroup/{id:int}", async Task<Results<BadRequest<string>, NoContent, NotFound<string>>> (Context context, int id) =>
{
    if (id < 1)
    {
        return TypedResults.BadRequest("Invalid task group id.");
    }

    if (await context.TaskGroups.Include(tg => tg.TaskItems).SingleOrDefaultAsync(tg => tg.Id == id) is TaskGroup group)
    {
        context.TaskGroups.Remove(group);
        await context.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    return TypedResults.NotFound("Unable to find task group to delete.");
});


app.Run();
