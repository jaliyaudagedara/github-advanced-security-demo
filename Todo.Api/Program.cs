using Microsoft.EntityFrameworkCore;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// ConfigureServices
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Map endpoint to get ToDos
app.MapGet("/Todos", async (ToDoDbContext dbContext) =>
{
    return Results.Ok(await dbContext.ToDos.ToListAsync());
});

// Map endpoint to get ToDo by id
app.MapGet("/Todos/{id}", async (ToDoDbContext dbContext, int id) =>
{
    ToDo? toDo = await dbContext.ToDos.FindAsync(id);
    if (toDo is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(toDo);
});

// Map endpoint to create ToDo
app.MapPost("/Todos", async (ToDoDbContext dbContext, ToDo toDo) =>
{
    await dbContext.ToDos.AddAsync(toDo);
    await dbContext.SaveChangesAsync();

    return Results.Created($"/Todos/{toDo.Id}", toDo);
});

// Map endpoint to update ToDo
app.MapPut("/Todos/{id}", async (ToDoDbContext dbContext, int id, ToDo toDo) =>
{
    if (await dbContext.ToDos.FindAsync(id) == null)
    {
        return Results.NotFound();
    }

    toDo.Id = id;
    dbContext.ToDos.Update(toDo);

    return Results.NoContent();
});

// Map endpoint to delete ToDo
app.MapDelete("/Todos/{id}", async (ToDoDbContext dbContext, int id) =>
{
    ToDo? toDo = await dbContext.ToDos.FindAsync(id);
    if (toDo is null)
    {
        return Results.NotFound();
    }

    dbContext.ToDos.Remove(toDo);
    await dbContext.SaveChangesAsync();

    return Results.NoContent();
});

await app.RunAsync();

// Create ToDo class
public class ToDo
{
    public int Id { get; set; }

    public string Title { get; set; }

    public bool IsDone { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime? CompletedDateTime { get; set; }
}

// Create ToDoDbContext with Entity Framework
public class ToDoDbContext : DbContext
{
    public DbSet<ToDo> ToDos { get; set; }

    public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
        : base(options)
    {
    }
}