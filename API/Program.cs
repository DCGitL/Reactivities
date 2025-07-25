using Application.Activities.Queries;
using Microsoft.EntityFrameworkCore;
using Persistence;
using FluentValidation;
using Application.Activities.DTOs;
using Application.Activities.Validators;
using Application.Core;
using API.Middleware;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Application.Interfaces;
using Infrastructure.Security;
using Infrastructure.Photos;
using API.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(opt =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
});
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors();
builder.Services.AddSignalR();
builder.Services.AddMediatR(x =>
{
    x.RegisterServicesFromAssemblyContaining<GetActivityList.Handler>();
    // x.AddBehavior(typeof(ValidationBehavior<,>));
});
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped<IUserAccessor, UserAccessor>();
builder.Services.AddTransient(typeof(MediatR.IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddValidatorsFromAssemblyContaining<CreateActivityValidator>();
builder.Services.AddTransient<ExceptionMiddleware>();
//Note this configuration must be placed before adding builder.Services.AddIdentityApiEndpoints<User>
builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
    options.SlidingExpiration = true;
});
builder.Services.AddIdentityApiEndpoints<User>(opt =>
{
    opt.User.RequireUniqueEmail = true;

})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("IsActivityHost", policy =>
    {
        policy.Requirements.Add(new IsHostRequirement());
    });
});
builder.Services.AddTransient<IAuthorizationHandler, IsHostRequirementHandler>();
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod()
.AllowCredentials()
.WithOrigins("http://localhost:3000", "https://localhost:3000"));

app.UseAuthentication();
app.UseAuthorization();


// Configure the HTTP request pipeline.
app.MapControllers();
app.MapGroup("api").MapIdentityApi<User>(); //api/login
app.MapHub<CommentHub>("/comments"); // SignalR hub for comments endpoint
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    // Apply migrations
    var context = services.GetRequiredService<AppDbContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    await context.Database.MigrateAsync();
    // Seed the database with initial data
    await DbInitializer.SeedDataAsync(context, userManager);

}
catch (Exception ex)
{
    // Handle migration errors
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration or seeding the database.");
}


app.Run();
