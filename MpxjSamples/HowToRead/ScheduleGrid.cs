using MPXJ.Net;

namespace MPXJ.Samples.HowToRead;

public class ScheduleGrid
{
    public void Read()
    {
        var reader = new UniversalProjectReader();
        var project = reader.Read("my-sample.schedule_grid");
    }
}
