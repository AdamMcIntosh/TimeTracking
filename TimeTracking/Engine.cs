using ConsoleTables;

namespace TimeTracking;

public static class Engine
{
    private static TimeTrackingContext db;

    public static void InitializeDb()
    {
        db = new TimeTrackingContext();
        db.Database.EnsureCreated();

        WriteToConsole($"Database path: {db.DbPath}.");
    }

    public static void WriteToConsole(string message)
    {
        var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
        Console.WriteLine($"[{timestamp}] {message}");
    }

    public static async Task Create(TrackingEntry tracking)
    {
        // Create
        WriteToConsole("Inserting a new entry");
        db.Add(tracking);
        await db.SaveChangesAsync();
    }

    public static async Task Read(int trackingId)
    {
        // Read
        WriteToConsole("Querying for an entry");
        var entry = await db.TimeTrackings
            .FindAsync(trackingId);
    }

    public static async Task Update(TrackingEntry tracking)
    {
        // Update
        WriteToConsole("Updating the entry");
        db.Update(tracking);
        await db.SaveChangesAsync();
    }

    public static async Task Delete(TrackingEntry tracking)
    {
        // Delete
        Console.WriteLine("Delete the entry");
        db.Remove(tracking);
        await db.SaveChangesAsync();
    }

    public static string FirstLetterToUpper(string str)
    {
        if (str.Length > 1)
            return char.ToUpper(str[0]) + str.Substring(1);

        return str.ToUpper();
    }
    
    public static void OutputHours()
    {
        WriteToConsole("Enter tracking month");
        var month = Console.ReadLine();
        WriteToConsole("Enter tracking year");
        var year = Console.ReadLine();
        if (string.IsNullOrEmpty(month))
        {
            month = DateTime.Now.Month.ToString();
        }
        if (string.IsNullOrEmpty(year))
        {
            year = DateTime.Now.Year.ToString();
        }
        var mondays = GetWeeks(month, year);

        var entries = new List<WeeklyActivityDto>();
        Console.WriteLine("***********************************************************************************************");
        Console.WriteLine("");
        foreach (var monday in mondays)
        {
            entries.AddRange(GetTimeTracking(monday));
        }
        ConsoleTable
            .From(entries)
            .Configure(o => o.NumberAlignment = Alignment.Right)
            .Write();
        Console.WriteLine("");
        Console.WriteLine("***********************************************************************************************");
        Console.WriteLine("");
        var minsToday = Engine.GetTotalTimeToday();
        var loggedToday = minsToday / 60;
        Engine.WriteToConsole($"Hours Logged today {DataUtil.GetDecimal(loggedToday)}");
        Console.WriteLine("");
        Console.WriteLine("***********************************************************************************************");
        Console.WriteLine("");
    }

    private static List<DateTime> GetWeeks(string month, string year)
    {
        DateTime date = DateTime.Today;
        date = new DateTime(DataUtil.GetInt32(year), DataUtil.GetInt32(month), 1);
        // first generate all dates in the month of 'date'
        var dates = Enumerable.Range(1, DateTime.DaysInMonth(date.Year, date.Month)).Select(n => new DateTime(date.Year, date.Month, n));
        // then filter the only the start of weeks
        var mondays = from d in dates
            where d.DayOfWeek == DayOfWeek.Monday
            select d;
        return mondays.ToList();
    }

    private static List<WeeklyActivityDto> GetTimeTracking(DateTime monday)
    {
        var friday = monday.AddDays(4);
        var period = $"{monday.ToShortDateString()} - {friday.ToShortDateString()}";
    
        var entries = from d in db.TimeTrackings
            where d.Date >= monday && d.Date <= friday
            select d;
    
        // Initialize values for each activity type
        decimal meetingDuration = 0;
        decimal supportDuration = 0;
        decimal alphaDuration = 0;
    
        // Process each unique type
        var uniqueTypes = entries.Select(e => e.Type).Distinct();
        foreach (var type in uniqueTypes)
        {
            var duration = entries.Where(e => e.Type.ToLower() == type.ToLower()).Sum(e => e.Duration);
            var totalAtcHours = DataUtil.GetDecimal(duration / 60);
        
            // Assign to the appropriate property based on type
            string typeString = DataUtil.GetString(type).ToLower();
            switch (typeString)
            {
                case "meeting":
                    meetingDuration = totalAtcHours;
                    break;
                case "support":
                    supportDuration = totalAtcHours;
                    break;
                case "alpha":
                    alphaDuration = totalAtcHours;
                    break;
                // Add more cases if there are other types
            }
        }
    
        // Calculate total hours
        decimal totalHours = meetingDuration + supportDuration + alphaDuration;
    
        // Create and return the consolidated DTO in a list
        return new List<WeeklyActivityDto>
        {
            new WeeklyActivityDto
            {
                Period = period,
                Meeting = meetingDuration,
                Support = supportDuration,
                Alpha = alphaDuration,
                Total = totalHours
            }
        };
    }

    private static decimal GetTotalTimeToday()
    {
        var today = DateTime.Today;
        var total = db.TimeTrackings.Where(x => x.Date == today).Sum(x => x.Duration);
        return total;
        
    }
}