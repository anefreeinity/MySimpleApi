using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? builder.Configuration["ConnectionStrings__DefaultConnection"];

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My Simple API", Version = "v1" });
});
    
var app = builder.Build();
app.Urls.Add("http://*:80");

// Enable middleware to serve generated Swagger as a JSON endpoint
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.)
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Simple API V1");
    c.RoutePrefix = string.Empty; // Set the Swagger UI at the app's root
});

app.MapGet("/", () => "Hello World!");

app.MapPost("/users", async (MyDbContext dbContext, User user) =>
{
    dbContext.Users.Add(user);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/users/{user.Id}", user);
});

// Endpoint to retrieve all users
app.MapGet("/users", async (MyDbContext dbContext) =>
{
    return await dbContext.Users.ToListAsync();
});

app.Run();
