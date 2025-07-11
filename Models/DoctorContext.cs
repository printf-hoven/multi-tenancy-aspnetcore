using Microsoft.EntityFrameworkCore;

namespace MultiTenancy.Models;

// Install-Package Microsoft.EntityFrameworkCore
// Install-Package Microsoft.EntityFrameworkCore.Sqlite
// Install-Package Microsoft.EntityFrameworkCore.Tools
public class DoctorContext : DbContext
{
  public DbSet<Doctor> Doctors { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    string databasePath = Path.Combine(Constants.DBASE_ROOT, "doctors.db");

    optionsBuilder.UseSqlite($"Data Source={databasePath}");
  }
}
