using System;
using System.Linq;
using MPXJ.Net;

public class CalendarSamples
{
    private const string DateFormat = "yyyy-MM-dd";

    public void Execute()
    {
        BasicOperations();
        CalendarHierarchy();
        CalendarUniqueId();
        DefaultCalendar();
        WorkingOrNonWorkingExceptions();
    }

    private void BasicOperations()
    {
        //
        // Create a default calendar
        //
        Console.WriteLine("Create a default calendar");
        var file = new ProjectFile();
        var calendar = file.AddDefaultBaseCalendar();
        Console.WriteLine("The calendar name is " + calendar.Name);
        Console.WriteLine();
        SimpleCalendarDump(calendar);

        //
        // Make Saturday a working day and Monday a non-working day
        //
        Console.WriteLine("Make Saturday a working day and Monday a non-working day");
        calendar.SetWorkingDay(DayOfWeek.Saturday, true);
        calendar.SetWorkingDay(DayOfWeek.Monday, false);
        SimpleCalendarDump(calendar);

        //
        // Show the "raw form" of the working hours for Tuesday
        //
        Console.WriteLine("Show the \"raw form\" of the working hours for Tuesday");
        var hours = calendar.GetCalendarHours(DayOfWeek.Tuesday);

        foreach (var h in hours)
        {
            Console.WriteLine(h);
        }
        Console.WriteLine();

        //
        // Show a formatted version of Tuesday's working hours
        //
        Console.WriteLine("Show a formatted version of Tuesday's working hours");
        Console.WriteLine(FormatDateRanges(hours));
        Console.WriteLine();

        //
        // Show a detailed dump of the whole calendar
        //
        Console.WriteLine("Show a detailed dump of the whole calendar");
        DetailedCalendarDump(calendar);

        //
        // Add some working hours to Saturday using constants supplied by MPXJ
        //
        Console.WriteLine("Add some working hours to Saturday using constants supplied by MPXJ");
        hours = calendar.GetCalendarHours(DayOfWeek.Saturday);
        hours.Add(ProjectCalendarDays.DefaultWorkingMorning);
        hours.Add(ProjectCalendarDays.DefaultWorkingAfternoon);
        DetailedCalendarDump(calendar);

        //
        // Create our own working hours for Saturday
        //
        Console.WriteLine("Create our own working hours for Saturday");
        var startTime = TimeOnly.Parse("09:00");
        var finishTime = TimeOnly.Parse("14:30");

        hours = calendar.GetCalendarHours(DayOfWeek.Saturday);
        hours.Clear();
        hours.Add(new TimeOnlyRange(startTime, finishTime));

        DetailedCalendarDump(calendar);

        //
        // Set up the same working hours, but use a helper method
        //
        Console.WriteLine("Set up the same working hours, but use a helper method");
        startTime = new TimeOnly(9, 0);
        finishTime = new TimeOnly(14, 30);
        hours.Clear();
        hours.Add(new TimeOnlyRange(startTime, finishTime));
        DetailedCalendarDump(calendar);

        //
        // Show how many working hours there are on Saturday
        //
        Console.WriteLine("Show how many working hours there are on Saturday");
        var duration = calendar.GetWork(DayOfWeek.Saturday, TimeUnit.Hours);
        Console.WriteLine(duration);
        Console.WriteLine();

        //
        // Let's try a naive approach to making Saturday 24 hours
        //
        Console.WriteLine("Let's try a naive approach to making Saturday 24 hours");
        startTime = new TimeOnly(0, 0);
        finishTime = new TimeOnly(0, 0);
        hours.Clear();
        hours.Add(new TimeOnlyRange(startTime, finishTime));
        Console.WriteLine(FormatDateRanges(calendar.GetCalendarHours(DayOfWeek.Saturday)));

        duration = calendar.GetWork(DayOfWeek.Saturday, TimeUnit.Hours);
        Console.WriteLine(duration);
        Console.WriteLine();

        //
        // Add an exception for a single day
        //
        Console.WriteLine("Add an exception for a single day");
        var exceptionDate = DateOnly.Parse("2022-05-10");

        var workingDate = calendar.IsWorkingDate(exceptionDate);
        Console.WriteLine(exceptionDate.ToString(DateFormat) + " is a " + (workingDate ? "working" : "non-working") + " day");

        var exception = calendar.AddCalendarException(exceptionDate);
        exception.Name = "A day off";

        workingDate = calendar.IsWorkingDate(exceptionDate);
        Console.WriteLine(exceptionDate.ToString(DateFormat) + " is a " + (workingDate ? "working" : "non-working") + " day");

        //
        // Make this a half-day
        //
        Console.WriteLine("Make this a half-day");
        startTime = new TimeOnly(8, 0);
        finishTime = new TimeOnly(12, 0);
        exception.Add(new TimeOnlyRange(startTime, finishTime));
        workingDate = calendar.IsWorkingDate(exceptionDate);
        Console.WriteLine(exceptionDate.ToString(DateFormat) + " is a " + (workingDate ? "working" : "non-working") + " day");

        Console.WriteLine("Working time on Tuesdays is normally "
            + calendar.GetWork(DayOfWeek.Tuesday, TimeUnit.Hours) + " but on "
            + exceptionDate.ToString(DateFormat) + " it is "
            + calendar.GetWork(exceptionDate, TimeUnit.Hours));
        Console.WriteLine();

        //
        // Add an exception affecting a number of days
        //
        Console.WriteLine("Add an exception affecting a number of days");
        DateDump(calendar, "2022-05-23", "2022-05-28");

        var exceptionStartDate = DateOnly.Parse("2022-05-24");
        var exceptionEndDate = DateOnly.Parse("2022-05-26");
        exception = calendar.AddCalendarException(exceptionStartDate, exceptionEndDate);
        startTime = new TimeOnly(9, 0);
        finishTime = new TimeOnly(13, 0);
        exception.Add(new TimeOnlyRange(startTime, finishTime));

        DateDump(calendar, "2022-05-23", "2022-05-28");

        //
        // Represent a "crunch" period in October.
        // Three weeks of 16 hour weekdays, with 8 hour days at weekends
        //
        Console.WriteLine("Represent a \"crunch\" period in October");
        var weekStart = DateOnly.Parse("2022-10-01");
        var weekEnd = DateOnly.Parse("2022-10-21");
        var week = calendar.AddWorkWeek();
        week.Name = "Crunch Time!";
        week.DateRange = new DateOnlyRange(weekStart, weekEnd);
        Enum.GetValues<DayOfWeek>().ToList().ForEach(d => week.SetWorkingDay(d, true));

        startTime = new TimeOnly(9, 0);
        finishTime = new TimeOnly(17, 0);
        var weekendHours = new TimeOnlyRange(startTime, finishTime);
        new[] { DayOfWeek.Saturday, DayOfWeek.Sunday }.ToList().ForEach(d => week.AddCalendarHours(d).Add(weekendHours));


        startTime = new TimeOnly(5, 0);
        finishTime = new TimeOnly(21, 0);
        var weekdayHours = new TimeOnlyRange(startTime, finishTime);
        new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday }.ToList().ForEach(d => week.AddCalendarHours(d).Add(weekdayHours));

        DetailedCalendarDump(week);

        DateDump(calendar, "2022-09-24", "2022-10-01");
        DateDump(calendar, "2022-10-01", "2022-10-08");

        //
        // Creating a recurring exception
        //
        var recurringData = new RecurringData();
        calendar.AddCalendarException(recurringData);

        recurringData.RecurrenceType = RecurrenceType.Yearly;
        recurringData.Occurrences = 5;
        recurringData.DayNumber = 1;
        recurringData.MonthNumber = 1;
        recurringData.StartDate = DateOnly.Parse("2023-01-01");
        Console.WriteLine(recurringData);
    }

    private void CalendarHierarchy()
    {
        var file = new ProjectFile();
        var parentCalendar = file.AddDefaultBaseCalendar();
        var christmasDay = DateOnly.Parse("2023-12-25");
        parentCalendar.AddCalendarException(christmasDay);

        var childCalendar = file.AddDefaultDerivedCalendar();
        childCalendar.Parent = parentCalendar;

        Console.WriteLine(christmasDay + " is a working day: " + childCalendar.IsWorkingDate(christmasDay));
        Console.WriteLine();

        parentCalendar.SetCalendarDayType(DayOfWeek.Tuesday, DayType.NonWorking);
        Console.WriteLine("Is " + DayOfWeek.Tuesday + " a working day: " + childCalendar.IsWorkingDay(DayOfWeek.Tuesday));
        Console.WriteLine();

        SimpleCalendarDump(parentCalendar);
        SimpleCalendarDump(childCalendar);

        childCalendar.SetCalendarDayType(DayOfWeek.Tuesday, DayType.Working);
        var startTime = new TimeOnly(9, 0);
        var finishTime = new TimeOnly(12, 30);
        childCalendar.AddCalendarHours(DayOfWeek.Tuesday).Add(new TimeOnlyRange(startTime, finishTime));
    }

    private void SimpleCalendarDump(ProjectCalendarDays calendar)
    {
        foreach (var day in Enum.GetValues<DayOfWeek>())
        {
            var dayType = calendar.GetCalendarDayType(day);
            Console.WriteLine(day + " is a " + dayType + " day");
        }
        Console.WriteLine();
    }

    private void DetailedCalendarDump(ProjectCalendarDays calendar)
    {
        foreach (var day in Enum.GetValues<DayOfWeek>())
        {
            var dayType = calendar.GetCalendarDayType(day);
            Console.WriteLine(day
                + " is a " + dayType + " day ("
                + FormatDateRanges(calendar.GetCalendarHours(day)) + ")");
        }
        Console.WriteLine();
    }

    private string FormatDateRanges(ProjectCalendarHours hours)
    {
        return string.Join(", ", hours.Select(h => $"{h.Start?.ToString("HH:mm")}-{h.End?.ToString("HH:mm")}"));
    }

    private void DateDump(ProjectCalendar calendar, string startDate, string endDate)
    {
        var start = DateOnly.Parse(startDate);
        var end = DateOnly.Parse(endDate);

        for (var date = start; date < end; date = date.AddDays(1))
        {
            Console.WriteLine(date.ToString(DateFormat) + "\t" + calendar.GetWork(date, TimeUnit.Hours));
        }

        Console.WriteLine();
    }

    private void CalendarUniqueId()
    {
        var file = new ProjectFile();
        var calendar1 = file.AddCalendar();
        calendar1.Name = "Calendar 1";

        var calendar2 = file.AddCalendar();
        calendar2.Name = "Calendar 2";

        var calendar3 = file.AddCalendar();
        calendar3.Name = "Calendar 3";

        foreach (var c in file.Calendars)
        {
            Console.WriteLine(c.Name);
        }
        Console.WriteLine();

        foreach (var c in file.Calendars)
        {
            Console.WriteLine(c.Name + " (Unique ID: " + c.UniqueID + ")");
        }
        Console.WriteLine();

        var calendar = file.Calendars.GetByUniqueID(2);
        Console.WriteLine(calendar.Name);
    }

    private void DefaultCalendar()
    {
        var file = new ProjectFile();
        var calendar = file.AddDefaultBaseCalendar();
        file.DefaultCalendar = calendar;
        Console.WriteLine("The default calendar name is " + file.DefaultCalendar.Name);
        Console.WriteLine();
    }


    private void WorkingOrNonWorkingExceptions()
    {
        var file = new ProjectFile();
        var calendar = file.AddDefaultBaseCalendar();

        //
        // Add an exception without hours - this makes the day non-working
        //
        var nonWorkingException = calendar.AddCalendarException(DateOnly.Parse("2022-05-23"));
        Console.WriteLine("Exception represents a working day: " + (nonWorkingException.Count > 0));

        //
        // Add an exception with hours to make this day working but with non-default hours
        //
        var workingException = calendar.AddCalendarException(DateOnly.Parse("2022-05-24"));
        var startTime = new TimeOnly(9, 0);
        var finishTime = new TimeOnly(13, 0);
        workingException.Add(new TimeOnlyRange(startTime, finishTime));
        Console.WriteLine("Exception represents a working day: " + (nonWorkingException.Count > 0));
    }
}
