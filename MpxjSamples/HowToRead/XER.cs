using MPXJ.Net;

public class XER
{
    public void Read()
    {
        var reader = new UniversalProjectReader();
        var project = reader.Read("my-sample.xer");
    }
}
