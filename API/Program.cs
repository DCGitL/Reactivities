using Application.Activities.Queries;

using Persistence;
using FluentValidation;
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
using Microsoft.EntityFrameworkCore;
using Infrastructure.EmailServer;
using Infrastructure.Email;
using System.Net.Http.Headers;
using Infrastructure.SocialMedia.Login;
using Infrastructure.Weather.WeatherService;
using Infrastructure.TimeZone;
using API.Helper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(opt =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
});
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
builder.Services.AddFluentEmail("support@techtest.com").AddSmtpSender(
   host: builder.Configuration.GetValue<string>("EmailSettings:SmtpHost"),
   port: builder.Configuration.GetValue<int>("EmailSettings: SmtpPort")
);
builder.Services.AddTransient<ISmptEmailSender, SmptEmailSender>();
builder.Services.AddTransient<IEmail, EmailSender>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddHttpClient("GitHubClient", options =>
{
    var tokenBaseUrl = builder.Configuration.GetSection("Authentication:Github:TokenBaseUrl").Value!;
    options.BaseAddress = new Uri(tokenBaseUrl); //endpoint => /login/oauth/access_token
    options.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

});
builder.Services.AddScoped<IWeatherServiceMonitor, WeatherServiceMonitor>();
builder.Services.AddScoped<IGeoTimeZoneService, GeoTimeZoneService>();
builder.Services.AddHttpClient(HttpClientName.WeatherSerivceClient, options =>
{
    var weatherServiceBaseUrl = builder.Configuration.GetSection("WeatherApi:ApiBaseUrl").Value!;
    options.BaseAddress = new Uri(weatherServiceBaseUrl); //endpoint => /login/oauth/access_token
    options.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

});
builder.Services.AddHttpClient(HttpClientName.TimezoneServiceClient, options =>
{
    var timezoneServiceBaseUrl = builder.Configuration.GetSection("Timezone:ApiBaseUrl").Value!;
    options.BaseAddress = new Uri(timezoneServiceBaseUrl); //endpoint => /login/oauth/access_token
    options.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

});

//Note this configuration must be placed before adding builder.Services.AddIdentityApiEndpoints<User>
//Note configure the Identity used before setting up the cookie
//This way you override all the default configuration of the default cookie that is 
//created by the identity server
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;

}).AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication()
    .AddCookie();
// Configure cookie auth separately
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
     {
         context.Response.StatusCode = StatusCodes.Status401Unauthorized;
         return Task.CompletedTask;
     };
    options.LoginPath = "/api/account/login";
    options.Cookie.Name = "ActivitiesCookie";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
    options.SlidingExpiration = true;

});

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

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();

// Configure the HTTP request pipeline.
app.MapControllers();
//app.MapGroup("api"); //.MapIdentityApi<User>(); //api/login
app.MapHub<CommentHub>("/comments"); // SignalR hub for comments endpoint
app.MapFallbackToController("Index", "Fallback");
// using var scope = app.Services.CreateScope();
// var services = scope.ServiceProvider;
//try
//{
//    // Apply migrations
//    //var context = services.GetRequiredService<AppDbContext>();
//    //var userManager = services.GetRequiredService<UserManager<User>>();
//    //await context.Database.MigrateAsync();
//    //// Seed the database with initial data
//    //await DbInitializer.SeedDataAsync(context, userManager);

//}
//catch (Exception ex)
//{
//    // Handle migration errors
//    var logger = services.GetRequiredService<ILogger<Program>>();
//    logger.LogError(ex, "An error occurred during migration or seeding the database.");
//}


app.Run();
