// Install - Package Microsoft.EntityFrameworkCore
// Install-Package Microsoft.EntityFrameworkCore.Sqlite
// Install-Package Microsoft.EntityFrameworkCore.Tools
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MultiTenancy;
using MultiTenancy.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<DoctorContext>();

builder.Services.AddDbContext<PatientContext>();

builder.Services.AddRazorPages(opt =>
{

  // the first arg is area, second must be slash, 
  // and the third is the policy 
  opt.Conventions.AuthorizeAreaFolder("Members", "/", "policy_members");

});

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {

      options.LoginPath = "/Login";

      options.AccessDeniedPath = "/Index";

      options.ExpireTimeSpan = TimeSpan.FromMinutes(20);

      options.SlidingExpiration = true;

    }

   );

builder.Services
  .AddAuthorizationBuilder()
  .AddPolicy("policy_members", policy => policy.RequireRole("member"));

var app = builder.Build();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Logger.LogInformation("ENVIRONMENT IS: {env}", app.Environment.EnvironmentName);

using (var scope = app.Services.CreateScope())
{
  // create database directory, if doesn't exist
  Directory.CreateDirectory(Constants.DBASE_ROOT);

  DoctorContext? ctx = scope.ServiceProvider.GetService<DoctorContext>();

  if (true == ctx?.Database.GetPendingMigrations().Any())
  {
    ctx.Database.Migrate();

    app.Logger.LogInformation("Identity migrations applied!");

  }

  if (true != ctx?.Database.CanConnect())
  {
    app.Logger.LogError(
              @"have you applied migration?
Open package manager console and run these commands first:
Add-Migration Initial_Create -context DoctorContext
Add-Migration Initial_Create -context PatientContext
");

    return;
  }
}

app.Run();

//------- application constants ---------//
namespace MultiTenancy
{
  public class Constants
  {
    public const string DBASE_ROOT = "Database";
  }
}