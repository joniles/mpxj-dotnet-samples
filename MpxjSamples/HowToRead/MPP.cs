using MPXJ.Net;

public class MPP
{
    public void Read()
    {
        var reader = new UniversalProjectReader();
        var project = reader.Read("my-sample.mpp");
    }
}
