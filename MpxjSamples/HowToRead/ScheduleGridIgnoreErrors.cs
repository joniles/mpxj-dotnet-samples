using MPXJ.Net;

public class ScheduleGridIgnoreErrors
{
    public void Read()
    {
        var reader = new SageReader();
        reader.IgnoreErrors = false;
        var project = reader.Read("my-sample.schedule_grid");
    }
}
