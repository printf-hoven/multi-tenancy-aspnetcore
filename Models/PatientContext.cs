using Microsoft.EntityFrameworkCore;

namespace MultiTenancy.Models;

public class PatientContext(IHttpContextAccessor _httpContext) : DbContext
{
  public DbSet<Patient> Patients { get; set; }

  public static string GetDoctorSpecificDatabaseFolder(IHttpContextAccessor _httpContext)
  {
    // any intermediate function can be called to 
    // set a difficult to guess name of this folder, if required
    return Path.Combine(
      Constants.DBASE_ROOT,
      $"DataOf_{_httpContext.HttpContext?.User.Identity?.Name ?? "_"}");
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    string databaseFilePath = Path.Combine(
      GetDoctorSpecificDatabaseFolder(_httpContext),
      "patients.db");

    optionsBuilder.UseSqlite($"Data Source={databaseFilePath}");
  }
}