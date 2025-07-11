using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MultiTenancy.Models;

namespace MultiTenancy.Areas.Members.Pages;

public class IndexModel(PatientContext _patientCtx, IHttpContextAccessor _httpContext, ILogger<IndexModel> _logger) : PageModel
{
  public async Task OnGet()
  {
    await EnsureDatabaseMigrationAndExistense();
  }

  public async Task EnsureDatabaseMigrationAndExistense()
  {
    string doctorsDatabaseFolder = PatientContext.GetDoctorSpecificDatabaseFolder(_httpContext);

    if (Directory.Exists(doctorsDatabaseFolder))
    {
      if (true == _patientCtx.Database.GetPendingMigrations().Any())
      {
        await _patientCtx.Database.MigrateAsync();

        _logger.LogInformation("Migrations applied to patients database!");
      }
    }
    else
    {
      Directory.CreateDirectory(doctorsDatabaseFolder);

      await _patientCtx.Database.MigrateAsync();

      await _patientCtx.Database.EnsureCreatedAsync();

      _logger.LogInformation("Created patients database!");
    }

    // get system time and check backup schedules, and perform them now!
  }
}

