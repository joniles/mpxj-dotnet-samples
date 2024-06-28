using MPXJ.Net;

public class Synchro
{
    public void Read()
    {
        var reader = new UniversalProjectReader();
        var project = reader.Read("my-sample.sp");
    }
}
