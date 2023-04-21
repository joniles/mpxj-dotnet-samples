﻿using java.lang;
using net.sf.mpxj;
using net.sf.mpxj.common;
using net.sf.mpxj.MpxjUtilities;

public class CalendarSamples
{
    private const string DateFormat = "dd/MM/yyyy";

    public void Execute()
    {
        BasicOperations();
        CalendarHierarchy();
        CalendarUniqueID();
        DefaultCalendar();
        WorkingOrNonWorkingExceptions();
    }

    private void BasicOperations()
    {
        //
        // Create a default calendar
        //
        System.Console.WriteLine("Create a default calendar");
        ProjectFile file = new ProjectFile();
        ProjectCalendar calendar = file.addDefaultBaseCalendar();
        System.Console.WriteLine("The calendar name is " + calendar.Name);
        System.Console.WriteLine();
        SimpleCalendarDump(calendar);

        //
        // Make Saturday a working day and Monday a non-working day
        //
        System.Console.WriteLine("Make Saturday a working day and Monday a non-working day");
        calendar.setWorkingDay(Day.SATURDAY, true);
        calendar.setWorkingDay(Day.MONDAY, false);
        SimpleCalendarDump(calendar);

        //
        // Show the "raw form" of the working hours for Tuesday
        //
        System.Console.WriteLine("Show the \"raw form\" of the working hours for Tuesday");
        var hours = calendar.getCalendarHours(Day.TUESDAY);

        foreach (var h in hours)
        {
            System.Console.WriteLine(h);
        }
        System.Console.WriteLine();

        //
        // Show a formatted version of Tuesday's working hours
        //
        System.Console.WriteLine("Show a formatted version of Tuesday's working hours");
        System.Console.WriteLine(FormatDateRanges(hours));
        System.Console.WriteLine();

        //
        // Show a detailed dump of the whole calendar
        //
        System.Console.WriteLine("Show a detailed dump of the whole calendar");
        DetailedCalendarDump(calendar);

        //
        // Add some working hours to Saturday using constants supplied by MPXJ
        //
        System.Console.WriteLine("Add some working hours to Saturday using constants supplied by MPXJ");
        hours = calendar.getCalendarHours(Day.SATURDAY);
        hours.add(ProjectCalendarDays.DEFAULT_WORKING_MORNING);
        hours.add(ProjectCalendarDays.DEFAULT_WORKING_AFTERNOON);
        DetailedCalendarDump(calendar);

        //
        // Create our own working hours for Saturday
        //
        System.Console.WriteLine("Create our own working hours for Saturday");
        var startTime = DateTime.ParseExact("09:00", "HH:mm", null).ToJavaDate();
        var finishTime = DateTime.ParseExact("14:30", "HH:mm", null).ToJavaDate();

        hours = calendar.getCalendarHours(Day.SATURDAY);
        hours.clear();
        hours.add(new DateRange(startTime, finishTime));

        DetailedCalendarDump(calendar);

        //
        // Set up the same working hours, but use a helper method
        //
        System.Console.WriteLine("Set up the same working hours, but use a helper method");
        startTime = DateHelper.getTime(9, 0);
        finishTime = DateHelper.getTime(14, 30);
        hours.clear();
        hours.add(new DateRange(startTime, finishTime));
        DetailedCalendarDump(calendar);

        //
        // Show how many working hours there are on Saturday
        //
        System.Console.WriteLine("Show how many working hours there are on Saturday");
        Duration duration = calendar.getWork(Day.SATURDAY, TimeUnit.HOURS);
        System.Console.WriteLine(duration);
        System.Console.WriteLine();

        //
        // Let's try a naive approach to making Saturday 24 hours
        //
        System.Console.WriteLine("Let's try a naive approach to making Saturday 24 hours");
        startTime = DateHelper.getTime(0, 0);
        finishTime = DateHelper.getTime(0, 0);
        hours.clear();
        hours.add(new DateRange(startTime, finishTime));
        System.Console.WriteLine(FormatDateRanges(calendar.getCalendarHours(Day.SATURDAY)));

        duration = calendar.getWork(Day.SATURDAY, TimeUnit.HOURS);
        System.Console.WriteLine(duration);
        System.Console.WriteLine();

        //
        // Make 24 hour days work by using an end time which is +1 day
        //
        System.Console.WriteLine("Make 24 hour days work by using an end time which is +1 day");
        startTime = DateTime.ParseExact("00:00", "HH:mm", null).ToJavaDate();
        finishTime = DateTime.ParseExact("00:00", "HH:mm", null).AddDays(1).ToJavaDate();

        hours.clear();
        hours.add(new DateRange(startTime, finishTime));
        System.Console.WriteLine(FormatDateRanges(calendar.getCalendarHours(Day.SATURDAY)));
        duration = calendar.getWork(Day.SATURDAY, TimeUnit.HOURS);
        System.Console.WriteLine(duration);
        System.Console.WriteLine();

        //
        // Add an exception for a single day
        //
        System.Console.WriteLine("Add an exception for a single day");
        var exceptionDate = DateTime.ParseExact("10/05/2022", DateFormat, null).ToJavaDate();

        bool workingDate = calendar.isWorkingDate(exceptionDate);
        System.Console.WriteLine(exceptionDate.ToDateTime().ToString(DateFormat) + " is a " + (workingDate ? "working" : "non-working") + " day");

        ProjectCalendarException exception = calendar.addCalendarException(exceptionDate);
        exception.Name = "A day off";

        workingDate = calendar.isWorkingDate(exceptionDate);
        System.Console.WriteLine(exceptionDate.ToDateTime().ToString(DateFormat) + " is a " + (workingDate ? "working" : "non-working") + " day");

        //
        // Make this a half-day
        //
        System.Console.WriteLine("Make this a half-day");
        startTime = DateHelper.getTime(8, 0);
        finishTime = DateHelper.getTime(12, 0);
        exception.add(new DateRange(startTime, finishTime));
        workingDate = calendar.isWorkingDate(exceptionDate);
        System.Console.WriteLine(exceptionDate.ToDateTime().ToString(DateFormat) + " is a " + (workingDate ? "working" : "non-working") + " day");

        System.Console.WriteLine("Working time on Tuesdays is normally "
            + calendar.getWork(Day.TUESDAY, TimeUnit.HOURS) + " but on "
            + exceptionDate.ToDateTime().ToString(DateFormat) + " it is "
            + calendar.getWork(exceptionDate, TimeUnit.HOURS));
        System.Console.WriteLine();

        //
        // Add an exception affecting a number of days
        //
        System.Console.WriteLine("Add an exception affecting a number of days");
        DateDump(calendar, "23/05/2022", "28/05/2022");

        var exceptionStartDate = DateTime.ParseExact("24/05/2022", DateFormat, null).ToJavaDate();
        var exceptionEndDate = DateTime.ParseExact("26/05/2022", DateFormat, null).ToJavaDate();
        exception = calendar.addCalendarException(exceptionStartDate, exceptionEndDate);
        startTime = DateHelper.getTime(9, 0);
        finishTime = DateHelper.getTime(13, 0);
        exception.add(new DateRange(startTime, finishTime));

        DateDump(calendar, "23/05/2022", "28/05/2022");

        //
        // Represent a "crunch" period in October.
        // Three weeks of 16 hour weekdays, with 8 hour days at weekends
        //
        System.Console.WriteLine("Represent a \"crunch\" period in October");
        var weekStart = DateTime.ParseExact("01/10/2022", DateFormat, null).ToJavaDate();
        var weekEnd = DateTime.ParseExact("21/10/2022", DateFormat, null).ToJavaDate();
        calendar = file.addDefaultBaseCalendar();
        ProjectCalendarWeek week = calendar.addWorkWeek();
        week.Name = "Crunch Time!";
        week.DateRange = new DateRange(weekStart, weekEnd);
        Day.values().ToList().ForEach(d => week.SetWorkingDay(d, true));

        startTime = DateHelper.getTime(9, 0);
        finishTime = DateHelper.getTime(17, 0);
        DateRange weekendHours = new DateRange(startTime, finishTime);
        new[] { Day.SATURDAY, Day.SUNDAY }.ToList().ForEach(d => week.addCalendarHours(d).add(weekendHours));


        startTime = DateHelper.getTime(5, 0);
        finishTime = DateHelper.getTime(21, 0);
        DateRange weekdayHours = new DateRange(startTime, finishTime);
        new[] { Day.MONDAY, Day.TUESDAY, Day.WEDNESDAY, Day.THURSDAY, Day.FRIDAY }.ToList().ForEach(d => week.addCalendarHours(d).add(weekdayHours));

        DetailedCalendarDump(week);

        DateDump(calendar, "24/09/2022", "01/10/2022");
        DateDump(calendar, "01/10/2022", "08/10/2022");

        //
        // Creating a recurring exception
        //
        RecurringData recurringData = new RecurringData();
        exception = calendar.addCalendarException(recurringData);

        recurringData.RecurrenceType = RecurrenceType.YEARLY;
        recurringData.Occurrences = Integer.valueOf(5);
        recurringData.DayNumber = Integer.valueOf(1);
        recurringData.MonthNumber = Integer.valueOf(1);
        recurringData.StartDate = DateTime.ParseExact("01/01/2023", DateFormat, null).ToJavaDate();
        System.Console.WriteLine(recurringData);
    }

    private void CalendarHierarchy()
    {
        var file = new ProjectFile();
        var parentCalendar = file.addDefaultBaseCalendar();
        var christmasDay = DateTime.ParseExact("25/12/2023", DateFormat, null).ToJavaDate();
        parentCalendar.addCalendarException(christmasDay);

        var childCalendar = file.addDefaultDerivedCalendar();
        childCalendar.Parent = parentCalendar;

        System.Console.WriteLine(christmasDay + " is a working day: " + childCalendar.isWorkingDate(christmasDay));
        System.Console.WriteLine();

        parentCalendar.setCalendarDayType(Day.TUESDAY, DayType.NON_WORKING);
        System.Console.WriteLine("Is " + Day.TUESDAY + " a working day: " + childCalendar.isWorkingDay(Day.TUESDAY));
        System.Console.WriteLine();

        SimpleCalendarDump(parentCalendar);
        SimpleCalendarDump(childCalendar);

        childCalendar.setCalendarDayType(Day.TUESDAY, DayType.WORKING);
        var startTime = DateHelper.getTime(9, 0);
        var finishTime = DateHelper.getTime(12, 30);
        childCalendar.addCalendarHours(Day.TUESDAY).add(new DateRange(startTime, finishTime));
    }

    private void SimpleCalendarDump(ProjectCalendarDays calendar)
    {
        foreach (Day day in Day.values())
        {
            string dayType = calendar.getCalendarDayType(day).toString();
            System.Console.WriteLine(day + " is a " + dayType + " day");
        }
        System.Console.WriteLine();
    }

    private void DetailedCalendarDump(ProjectCalendarDays calendar)
    {
        foreach (Day day in Day.values())
        {
            string dayType = calendar.GetCalendarDayType(day).toString();
            System.Console.WriteLine(day
                + " is a " + dayType + " day ("
                + FormatDateRanges(calendar.GetCalendarHours(day)) + ")");
        }
        System.Console.WriteLine();
    }

    private string FormatDateRanges(ProjectCalendarHours hours)
    {
        var list = hours.ToIEnumerable<DateRange>().ToList();
        return string.Join(", ", list.Select(h => $"{h.Start.ToDateTime().ToString("HH:mm")}-{h.End.ToDateTime().ToString("HH:mm")}"));
    }

    private void DateDump(ProjectCalendar calendar, string startDate, string endDate)
    {
        var start = DateTime.ParseExact(startDate, DateFormat, null);
        var end = DateTime.ParseExact(endDate, DateFormat, null);

        for (var date = start; date < end; date = date.AddDays(1))
        {
            System.Console.WriteLine(date.ToString(DateFormat) + "\t" + calendar.GetWork(date.ToJavaDate(), TimeUnit.HOURS));
        }

        System.Console.WriteLine();
    }

    private void CalendarUniqueID()
    {
        var file = new ProjectFile();
        var calendar1 = file.addCalendar();
        calendar1.Name = "Calendar 1";

        var calendar2 = file.addCalendar();
        calendar2.Name = "Calendar 2";

        var calendar3 = file.addCalendar();
        calendar3.Name = "Calendar 3";

        foreach (ProjectCalendar c in file.Calendars)
        {
            System.Console.WriteLine(c.Name);
        }
        System.Console.WriteLine();

        foreach (ProjectCalendar c in file.Calendars)
        {
            System.Console.WriteLine(c.Name + " (Unique ID: " + c.UniqueID + ")");
        }
        System.Console.WriteLine();

        ProjectCalendar calendar = (ProjectCalendar)file.Calendars.GetByUniqueID(java.lang.Integer.valueOf(2));
        System.Console.WriteLine(calendar.Name);
    }

    private void DefaultCalendar()
    {
        var file = new ProjectFile();
        var calendar = file.addDefaultBaseCalendar();
        file.DefaultCalendar = calendar;
        System.Console.WriteLine("The default calendar name is " + file.DefaultCalendar.Name);
        System.Console.WriteLine();
    }


    private void WorkingOrNonWorkingExceptions()
    {
        var file = new ProjectFile();
        var calendar = file.addDefaultBaseCalendar();

        //
        // Add an exception without hours - this makes the day non-working
        //
        var nonWorkingException = calendar.addCalendarException(DateTime.ParseExact("23/05/2022", DateFormat, null).ToJavaDate());
        System.Console.WriteLine("Exception represents a working day: " + !nonWorkingException.isEmpty());

        //
        // Add an exception with hours to make this day working but with non-default hours
        //
        var workingException = calendar.addCalendarException(DateTime.ParseExact("24/05/2022", DateFormat, null).ToJavaDate());
        var startTime = DateHelper.getTime(9, 0);
        var finishTime = DateHelper.getTime(13, 0);
        workingException.add(new DateRange(startTime, finishTime));
        System.Console.WriteLine("Exception represents a working day: " + !workingException.isEmpty());
    }
}