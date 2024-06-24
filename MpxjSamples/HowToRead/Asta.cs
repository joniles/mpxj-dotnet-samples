using MPXJ.Net;

public class Asta
{
    public void Read()
    {
        var reader = new UniversalProjectReader();
        var project = reader.Read("my-sample.pp");
    }
}
