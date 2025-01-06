using MPXJ.Net;

namespace MpxjSamples.HowToRead;

public class ScheduleGridIgnoreErrors
{
    public void Read()
    {
        var reader = new SageReader();
        reader.IgnoreErrors = false;
        var project = reader.Read("my-sample.schedule_grid");
    }
}
