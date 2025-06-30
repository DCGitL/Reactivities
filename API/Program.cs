using Application.Activities.Queries;
using Microsoft.EntityFrameworkCore;
using Persistence;
using FluentValidation;
using Application.Activities.DTOs;
using Application.Activities.Validators;
using Application.Core;
using API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors();
builder.Services.AddMediatR(x =>
{
    x.RegisterServicesFromAssemblyContaining<GetActivityList.Handler>();
    // x.AddBehavior(typeof(ValidationBehavior<,>));
});
builder.Services.AddTransient(typeof(MediatR.IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddValidatorsFromAssemblyContaining<CreateActivityValidator>();
builder.Services.AddTransient<ExceptionMiddleware>();

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod()
.WithOrigins("http://localhost:3000", "https://localhost:3000"));


// Configure the HTTP request pipeline.

app.MapControllers();
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    // Apply migrations
    var context = services.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync();
    // Seed the database with initial data
    await DbInitializer.SeedDataAsync(context);

}
catch (Exception ex)
{
    // Handle migration errors
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration or seeding the database.");
}


app.Run();
