using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MenuItem = MNightWorks.Shared.Models.MenuItem; // same alias trick as MainPage.xaml.cs

namespace MNightWorks.Mobile;

// A second, separate database from the API's — this one lives on the device itself,
// as a local cache, not the "real" source of truth. The server's database still is.
public class LocalDbContext : DbContext
{
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // FileSystem.AppDataDirectory is a MAUI-provided path that always points to
        // a safe, writable folder for this app's private data — and it automatically
        // resolves to the correct real location on Windows, Android, or iOS, without
        // you needing to write different code for each platform.
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "local_cache.db");
        options.UseSqlite($"Data Source={dbPath}");
    }
}