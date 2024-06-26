using MPXJ.Net;

public class MSPDI
{
    public void Read()
    {
        var reader = new UniversalProjectReader();
        var project = reader.Read("my-sample.xml");
    }
}
