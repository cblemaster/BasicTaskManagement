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

app.MapGet("/taskgroup/manage", Results<NotFound<string>, Ok<IEnumerable<TaskGroupDTO>>> (Context context) =>
{
    IEnumerable<TaskGroupDTO> groups = EntityToDTO.MapTaskGroupCollection(context.TaskGroups.Include(tg => tg.TaskItems.OrderByDescending(ti => ti.DueDate)));
    return groups is null || !groups.Any()
        ? TypedResults.NotFound("No task groups found.")
        : TypedResults.Ok(groups.OrderBy(g => g.Name).AsEnumerable());
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

    if (context.TaskGroups.Select(tg => tg.Name).Contains(createGroup.Name))
    {
        return TypedResults.BadRequest($"Task group name {createGroup.Name} is already used.");
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
        if (group.TaskItems.Count != 0)
        {
            return TypedResults.BadRequest("Cannot delete task group since it contains task items.");
        }

        context.TaskGroups.Remove(group);
        await context.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    return TypedResults.NotFound("Unable to find task group to delete.");
});

app.MapPut("/taskgroup/{id:int}", async Task<Results<BadRequest<string>, NoContent>> (Context context, int id, CreateTaskGroupDTO dto) =>
{
    if (dto is null)
    {
        return TypedResults.BadRequest("No task group to update provided.");
    }

    if (id < 1 || id != dto.Id)
    {
        return TypedResults.BadRequest("Invalid task group id.");
    }

    foreach (TaskGroup group in context.TaskGroups)
    {
        if (group.Id != dto.Id && group.Name == dto.Name)
        {
            return TypedResults.BadRequest($"Task group name {dto.Name} is already used.");
        }
    }

    ValidationResult validationResult = dto.Validate();

    if (!validationResult.IsValid)
    {
        return TypedResults.BadRequest(validationResult.ErrorMessage);
    }

    TaskGroup entity = (await context.TaskGroups.SingleOrDefaultAsync(g => g.Id == id))!;

    if (entity is null)
    {
        return TypedResults.BadRequest("Unable to find task group to update.");
    }

    entity = DTOToEntity.MapUpdateTaskGroup(dto, entity);
    await context.SaveChangesAsync();

    return TypedResults.NoContent();
});

app.MapGet("/taskitem", Results<NotFound<string>, Ok<IEnumerable<TaskItemDTO>>> (Context context) =>
{
    IEnumerable<TaskItemDTO> items = EntityToDTO.MapTaskItemCollection(context.TaskItems.Include(ti => ti.TaskGroup).OrderByDescending(ti => ti.DueDate));
    return items is null || !items.Any()
        ? TypedResults.NotFound("No task items found.")
        : TypedResults.Ok(items.AsEnumerable());
});

app.MapGet("/taskitem/important", Results<NotFound<string>, Ok<IEnumerable<TaskItemDTO>>> (Context context) =>
{
    IEnumerable<TaskItemDTO> items = EntityToDTO.MapTaskItemCollection(context.TaskItems.Include(ti => ti.TaskGroup).Where(ti => ti.IsImportant && !ti.IsComplete).OrderByDescending(ti => ti.DueDate).ThenBy(ti => ti.Name));
    return items is null || !items.Any()
        ? TypedResults.NotFound("No task items found.")
        : TypedResults.Ok(items.AsEnumerable());
});

app.MapGet("/taskitem/important/showcomplete", Results<NotFound<string>, Ok<IEnumerable<TaskItemDTO>>> (Context context) =>
{
    IEnumerable<TaskItemDTO> items = EntityToDTO.MapTaskItemCollection(context.TaskItems.Include(ti => ti.TaskGroup).Where(ti => ti.IsImportant).OrderByDescending(ti => ti.DueDate).ThenBy(ti => ti.Name));
    return items is null || !items.Any()
        ? TypedResults.NotFound("No task items found.")
        : TypedResults.Ok(items.AsEnumerable());
});

app.MapGet("/taskitem/duetoday", Results<NotFound<string>, Ok<IEnumerable<TaskItemDTO>>> (Context context) =>
{
    IEnumerable<TaskItemDTO> items = EntityToDTO.MapTaskItemCollection(context.TaskItems.Include(ti => ti.TaskGroup).Where(ti => ti.DueDate.HasValue && ti.DueDate.Value.Date == DateTime.Today && !ti.IsComplete).OrderBy(ti => ti.Name));
    return items is null || !items.Any()
        ? TypedResults.NotFound("No task items found.")
        : TypedResults.Ok(items.AsEnumerable());
});

app.MapGet("/taskitem/duetoday/showcomplete", Results<NotFound<string>, Ok<IEnumerable<TaskItemDTO>>> (Context context) =>
{
    IEnumerable<TaskItemDTO> items = EntityToDTO.MapTaskItemCollection(context.TaskItems.Include(ti => ti.TaskGroup).Where(ti => ti.DueDate.HasValue && ti.DueDate.Value.Date == DateTime.Today).OrderBy(ti => ti.Name));
    return items is null || !items.Any()
        ? TypedResults.NotFound("No task items found.")
        : TypedResults.Ok(items.AsEnumerable());
});

app.MapGet("/taskitem/completed", Results<NotFound<string>, Ok<IEnumerable<TaskItemDTO>>> (Context context) =>
{
    IEnumerable<TaskItemDTO> items = EntityToDTO.MapTaskItemCollection(context.TaskItems.Include(ti => ti.TaskGroup).Where(ti => ti.IsComplete).OrderByDescending(ti => ti.DueDate).ThenBy(ti => ti.Name));
    return items is null || !items.Any()
        ? TypedResults.NotFound("No task items found.")
        : TypedResults.Ok(items.AsEnumerable());
});

app.MapGet("/taskitem/{id:int}", async Task<Results<BadRequest<string>, Ok<TaskItemDTO>, NotFound<string>>> (Context context, int id) =>
{
    if (id < 1)
    {
        return TypedResults.BadRequest("Invalid task item id.");
    }
    if (await context.TaskItems.Include(ti => ti.TaskGroup).SingleOrDefaultAsync(ti => ti.Id == id) is TaskItem taskitem)
    {
        TaskItemDTO item = EntityToDTO.MapTaskItem(taskitem);
        return TypedResults.Ok(item);
    }
    return TypedResults.NotFound($"No task item with id {id} found.");
});

app.MapPost("/taskitem", async Task<Results<BadRequest<string>, Created<TaskItemDTO>>> (Context context, CreateUpdateTaskItemDTO createItem) =>
{
    if (createItem is null)
    {
        return TypedResults.BadRequest("No item to create provided.");
    }

    ValidationResult validationResult = createItem.Validate();

    if (!validationResult.IsValid)
    {
        return TypedResults.BadRequest(validationResult.ErrorMessage);
    }

    TaskItem itemToCreate = DTOToEntity.MapCreateUpdateTaskItem(createItem);
    context.TaskItems.Add(itemToCreate);
    await context.SaveChangesAsync();

    itemToCreate.TaskGroup = await context.TaskGroups.SingleOrDefaultAsync(tg => tg.Id == itemToCreate.TaskGroupId) ?? new();
    return TypedResults.Created($"/taskitem/{itemToCreate.Id}", EntityToDTO.MapTaskItem(itemToCreate));
});

app.MapDelete("/taskitem/{id:int}", async Task<Results<BadRequest<string>, NoContent, NotFound<string>>> (Context context, int id) =>
{
    if (id < 1)
    {
        return TypedResults.BadRequest("Invalid task item id.");
    }

    if (await context.TaskItems.Include(ti => ti.TaskGroup).SingleOrDefaultAsync(ti => ti.Id == id) is TaskItem item)
    {
        context.TaskItems.Remove(item);
        await context.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    return TypedResults.NotFound("Unable to find format to delete.");
});

app.MapPut("/taskitem/{id:int}", async Task<Results<BadRequest<string>, NoContent>> (Context context, int id, CreateUpdateTaskItemDTO dto) =>
{
    if (dto is null)
    {
        return TypedResults.BadRequest("No task item to update provided.");
    }

    if (id < 1 || id != dto.Id)
    {
        return TypedResults.BadRequest("Invalid task item id.");
    }

    ValidationResult validationResult = dto.Validate();

    if (!validationResult.IsValid)
    {
        return TypedResults.BadRequest(validationResult.ErrorMessage);
    }

    TaskItem check = (await context.TaskItems.SingleOrDefaultAsync(ti => ti.Id == id))!;
    if (check is not null && check.IsComplete)
    {
        return TypedResults.BadRequest("Completed task items cannot be updated.");
    }

    TaskItem entity = DTOToEntity.MapCreateUpdateTaskItem(dto);
    context.TaskItems.Entry(entity).State = EntityState.Modified;
    await context.SaveChangesAsync();

    return TypedResults.NoContent();
});

app.Run();
