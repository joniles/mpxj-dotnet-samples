using net.sf.mpxj;
using net.sf.mpxj.MpxjUtilities;
using net.sf.mpxj.reader;

/// <summary>
/// Demonstrate how a calendar's exceptions can be separated out into the
/// individual affected days.
/// </summary>
public class ProjectCalendarExceptionsToDates
{
    public void Execute()
    {
        ProjectFile file = new UniversalProjectReader().read("example.mpp");
        foreach (ProjectCalendar calendar in file.Calendars)
        {
            processCalendar(calendar);
        }
    }

    private void processCalendar(ProjectCalendar calendar)
    {        
        if (calendar.CalendarExceptions.isEmpty())
        {
            System.Console.WriteLine($"Calendar {calendar.Name} has no exceptions");
        }
        else
        {
            System.Console.WriteLine($"Calendar {calendar.Name} exceptions:");
            foreach(ProjectCalendarException exception in calendar.CalendarExceptions.ToIEnumerable())
            {
                processCalendarException(exception);
            }
        }
    }

    private void processCalendarException(ProjectCalendarException exception)
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

        foreach (ProjectCalendarException expandedException in exception.ExpandedExceptions.ToIEnumerable())
        {
            var fromDate = expandedException.FromDate.ToDateTime();
            var toDate = expandedException.ToDate.ToDateTime();
            while (fromDate < toDate)
            {
                days.Add(fromDate.ToString("MM/dd/yyyy"));
                fromDate = fromDate.AddDays(1);
            }
        }

        System.Console.WriteLine($"\tDays: {String.Join(", ", days)}");
    }
}
