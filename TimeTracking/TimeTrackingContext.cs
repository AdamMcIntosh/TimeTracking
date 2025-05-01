using Microsoft.EntityFrameworkCore;

namespace TimeTracking;

public class TimeTrackingContext: DbContext
{
    public DbSet<TrackingEntry> TimeTrackings { get; set; }

    public string DbPath { get; }

    public TimeTrackingContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "timetracking.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}

public class TrackingEntry
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Description { get; set; }
    public decimal Duration { get; set; }
    public DateTime Date { get; set; }
    
}

public class WeeklyActivityDto
{
    public string Period { get; set; }
    public decimal Meeting { get; set; }
    public decimal Support { get; set; }
    public decimal Alpha { get; set; }
    public decimal Total { get; set; }
}
