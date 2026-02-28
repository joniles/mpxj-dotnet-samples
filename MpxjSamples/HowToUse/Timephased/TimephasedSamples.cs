using System;
using System.Collections.Generic;
using MPXJ.Net;


namespace MpxjSamples.HowToUse.Timephased;

public class TimephasedSamples
{
    public void TimescaleRanges()
    {
        {
            // Single range
            var today = new DateTimeRange(
                new DateTime(2026, 2, 18),
                new DateTime(2026, 2, 19));
        }

        {
            // Multiple ranges
            var day1 = new DateTimeRange(
                new DateTime(2026, 2, 18),
                new DateTime(2026, 2, 19));
            var day2 = new DateTimeRange(
                new DateTime(2026, 2, 19),
                new DateTime(2026, 2, 20));
            var timescale = new List<DateTimeRange> { day1, day2 };
        }

        {
            // Timescale with segment count
            var startDate = new DateTime(2026, 2, 16);
            var ranges = new TimescaleUtility().createTimescale(startDate, 5, TimescaleUnits.DAYS);
        }
    }
}