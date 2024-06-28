using MPXJ.Net;

public class SureTrak
{
    public void Read()
    {
        var reader = new UniversalProjectReader();
        var project = reader.Read("my-sample.stx");
    }
}
