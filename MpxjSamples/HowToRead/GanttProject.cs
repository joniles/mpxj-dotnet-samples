using MPXJ.Net;

public class GanttProject
{
    public void Read()
    {
        var reader = new UniversalProjectReader();
        var project = reader.Read("my-sample.gan");
    }
}
