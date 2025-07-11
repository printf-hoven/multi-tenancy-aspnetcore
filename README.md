# multi-tenancy-aspnetcore
Multi-Tenancy in ASPNET Core - a simpler approach, without the need for any un-necessary nuget packages.

See the blog page - [The Problem of Multi-tenancy in ASP.NET Core and a Suggested Solution](https://hoven.in/aspnet-core/multitenancy-problem-and-solutions.html)

## Add these nugets
Add these nugets for EFCore and SQLite. 
```
Install-Package Microsoft.EntityFrameworkCore
Install-Package Microsoft.EntityFrameworkCore.Sqlite
Install-Package Microsoft.EntityFrameworkCore.Tools
```

## Migrations
Following have <ins>already been added</ins>
```
Add-Migration Initial_Create -context DoctorContext
Add-Migration Initial_Create -context PatientContext
```

You can make changes to the models from time to time, and run these commands to set migrations
```
Add-Migration [name-here] -context DoctorContext
Add-Migration [name-here] -context PatientContext
```

The C# code will automatically apply the first migration when the app is run the next time; the second will be applied to the database of each doctor whenever the respective doctor logs in the next time.
