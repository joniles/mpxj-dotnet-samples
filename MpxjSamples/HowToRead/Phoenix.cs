using MPXJ.Net;

public class Phoenix
{
    public void Read()
    {
        var reader = new UniversalProjectReader();
        var project = reader.Read("my-sample.ppx");
    }
}
