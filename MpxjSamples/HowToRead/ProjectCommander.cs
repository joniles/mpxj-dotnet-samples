using MPXJ.Net;

public class ProjectCommander
{
    public void Read()
    {
        var reader = new UniversalProjectReader();
        var project = reader.Read("my-sample.pc");
    }
}
