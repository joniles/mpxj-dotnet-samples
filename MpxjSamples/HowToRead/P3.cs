using MPXJ.Net;

public class P3
{
    public void Read()
    {
        var reader = new UniversalProjectReader();
        var project = reader.Read("my-sample.prx");
    }
}
