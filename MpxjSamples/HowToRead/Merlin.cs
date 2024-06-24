using MPXJ.Net;

public class Merlin
{
    public void Read()
    {
        var reader = new UniversalProjectReader();
        var project = reader.Read("my-sample-merlin-project");
    }
}
