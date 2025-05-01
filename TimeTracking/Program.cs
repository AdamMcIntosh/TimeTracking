using System.Diagnostics;
using TimeTracking;

// Console.BackgroundColor = ConsoleColor.DarkGreen;
// Console.ForegroundColor = ConsoleColor.White;
Engine.WriteToConsole("Initializing database");
Engine.InitializeDb();

var wasPaused = false;

Engine.OutputHours();

Engine.WriteToConsole("Enter 'quit' to exit");
Engine.WriteToConsole("Automatic time tracking or manual time tracking? (automatic/manual)");

var answer = Console.ReadLine();
var description = "";   
var project = "";
var stopwatch = Stopwatch.StartNew();
while (answer != "quit")
{
    if (answer == "automatic" || wasPaused)
    {
        if (!wasPaused)
        {
            stopwatch.Stop();
            stopwatch.Reset();
            Engine.WriteToConsole("Enter tracking item");
            answer = Console.ReadLine();
            
            Engine.WriteToConsole("Enter a description");
            description = Console.ReadLine();
            
            Engine.WriteToConsole("Enter the project");
            project = Console.ReadLine();
            
            if (string.IsNullOrEmpty(project))
                project = "Support";
        
            project = Engine.FirstLetterToUpper(project);
            
            stopwatch.Start();
        }
        
        Engine.WriteToConsole("Press 'q' to exit the timer or 'p to pause");
        var keyPressed = Console.ReadKey().Key.ToString();
        Engine.WriteToConsole($"Key pressed: {keyPressed}");
        switch (keyPressed.ToLower())
        {
            case "p":
            {
                stopwatch.Stop();
                Engine.WriteToConsole("Timer paused press enter to continue");
                if (Console.ReadKey().Key == ConsoleKey.Enter)
                {
                    wasPaused = true;
                    stopwatch.Start();
                    Engine.WriteToConsole("Timer resumed");
                }

                break;
            }
            case "q":
            {
                wasPaused = false;
                stopwatch.Stop();
                var duration = stopwatch.Elapsed.Minutes;
                Engine.WriteToConsole($"Duration: {duration}");

                var tracking = new TrackingEntry
                {
                    Name = answer,
                    Duration = DataUtil.GetDecimal(duration.ToString()),
                    Description = description,
                    Date = DateTime.Today,
                    Type = project
                };

                await Engine.Create(tracking);
                Engine.OutputHours();
                Engine.WriteToConsole("Automatic time tracking or manual time tracking? (automatic/manual)");
                answer = Console.ReadLine();
                break;
            }
            default:
                answer = "automatic";
                break;
        }
    }
    else if (answer == "manual")
    {
        Engine.WriteToConsole("Enter tracking item");
        var item = Console.ReadLine();
        Engine.WriteToConsole("Enter a description");
        description = Console.ReadLine();
        
        Engine.WriteToConsole("Enter the project");
        project = Console.ReadLine();
        
        if (string.IsNullOrEmpty(project))
            project = "Support";
        
        project = Engine.FirstLetterToUpper(project);
        
        Engine.WriteToConsole("Enter duration in minutes");
        var duration = Console.ReadLine();
        
        Engine.WriteToConsole("Enter date");
        var date = Console.ReadLine();
        
        DateTime.TryParse(date ?? string.Empty, out var dateTimeResult);
        var tracking = new TrackingEntry
        {
            Name = item,
            Duration = DataUtil.GetDecimal(duration),
            Description = description,
            Date = dateTimeResult,
            Type = project
        };

        await Engine.Create(tracking);
        Engine.OutputHours();
        Engine.WriteToConsole("Automatic time tracking or manual time tracking? (automatic/manual)");
        answer = Console.ReadLine();
    }
    else
    {
        Engine.OutputHours(); 
        Engine.WriteToConsole("Automatic time tracking or manual time tracking? (automatic/manual)");
        answer = Console.ReadLine();
    }
}