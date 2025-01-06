using MPXJ.Net;

/// <summary>
/// Demonstrate how a calendar's exceptions can be separated out into the
/// individual affected days.
/// </summary>
public class ProjectCalendarExceptionsToDates
{
    public void Execute(string filename)
    {
        var file = new UniversalProjectReader().Read(filename);
        foreach (var calendar in file.Calendars)
        {
            ProcessCalendar(calendar);
        }
    }

    private void ProcessCalendar(ProjectCalendar calendar)
    {        
        if (calendar.CalendarExceptions.Count == 0)
        {
            System.Console.WriteLine($"Calendar {calendar.Name} has no exceptions");
        }
        else
        {
            System.Console.WriteLine($"Calendar {calendar.Name} exceptions:");
            foreach(var exception in calendar.CalendarExceptions)
            {
                ProcessCalendarException(exception);
            }
        }
    }

    private void ProcessCalendarException(ProjectCalendarException exception)
    {
        System.Console.WriteLine($"Exception Name: {exception.Name}");

        // We're using ExpandedExceptions here to ensure we deal with
        // working weeks and recurring exceptions. The list
        // returned from this property could either contain a ProjectCalendarException
        // representing a contiguous range of dates, or it may contain a number
        // ProjectCalendarException instances represneting separate days.
        // As a demonstration we're just collecting the string representations of
        // these dates into a list and concatenating them for display.
        // NOTE: we're not distinguishing here between exceptions which
        // change a working day into a non-working day, a non-working day into
        // a working day, or change a working day's hours.
        var days = new List<string>();

        foreach (var expandedException in exception.ExpandedExceptions)
        {
            var fromDate = expandedException.FromDate ?? throw new ArgumentException("exception missing from date");
            var toDate = expandedException.ToDate ?? throw new ArgumentException("exception missing to date");

            while (fromDate <= toDate)
            {
                days.Add(fromDate.ToString("yyyy-MM-dd"));
                fromDate = fromDate.AddDays(1);
            }
        }

        
        System.Console.WriteLine($"\tDays: {string.Join(", ", days)}");
    }
}
