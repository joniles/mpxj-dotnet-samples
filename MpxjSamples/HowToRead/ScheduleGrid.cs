using MPXJ.Net;

namespace MpxjSamples.HowToRead;

public class ScheduleGrid
{
    public void Read()
    {
        var reader = new UniversalProjectReader();
        var project = reader.Read("my-sample.schedule_grid");
    }
}
