using MPXJ.Net;

public class OpenPlan
{
    public void Read()
    {
        var reader = new UniversalProjectReader();
        var project = reader.Read("my-sample.bk3");
    }
}
