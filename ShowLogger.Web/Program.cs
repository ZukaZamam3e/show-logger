using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using ShowLogger.Data.Context;
using ShowLogger.Models;
using ShowLogger.Models.Api;
using ShowLogger.Store.Repositories;
using ShowLogger.Store.Repositories.Interfaces;
using ShowLogger.Web.Data;
using ShowLogger.Web.TokenProviders;
using System.Configuration;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

if(Debugger.IsAttached)
{
    builder.Configuration.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);
}

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("IdentityConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

string showLoggerConnectionString = builder.Configuration.GetConnectionString("ShowLoggerConnection");
builder.Services.AddDbContext<ShowLoggerDbContext>(x => x.UseMySql(showLoggerConnectionString, ServerVersion.AutoDetect(showLoggerConnectionString)), ServiceLifetime.Transient);
builder.Services.AddScoped<IWatchedShowsRepository, WatchedShowsRepository>();
builder.Services.AddScoped<IFriendRepository, FriendRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IInfoRepository, InfoRepository>();
builder.Services.AddScoped<IDownloadRepository, DownloadRepository>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

ApisConfig apisConfig = new ApisConfig();
builder.Configuration.GetSection("Apis").Bind(apisConfig);
builder.Services.AddSingleton(apisConfig);

ShowLoggerSettings showLoggerSettings = new ShowLoggerSettings();
builder.Configuration.GetSection("ShowLoggerSettings").Bind(showLoggerSettings);
builder.Services.AddSingleton(showLoggerSettings);


builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Tokens.EmailConfirmationTokenProvider = "emailconfirmation";
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddTokenProvider<EmailConfirmationTokenProvider<ApplicationUser>>("emailconfirmation");

builder.Services.Configure<EmailConfirmationTokenProviderOptions>(opt =>
     opt.TokenLifespan = TimeSpan.FromDays(3));

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddControllers(config =>
{
    var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build();
    config.Filters.Add(new AuthorizeFilter(policy));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
