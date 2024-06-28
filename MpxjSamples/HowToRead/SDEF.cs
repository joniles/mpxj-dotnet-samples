using MPXJ.Net;

public class SDEF
{
    public void Read()
    {
        var reader = new UniversalProjectReader();
        var project = reader.Read("my-sample.sdef");
    }
}
