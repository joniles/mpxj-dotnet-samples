using MPXJ.Net;

public class MPDFile
{
    public void Read()
    {
        var reader = new UniversalProjectReader();
        var project = reader.Read("my-sample.mpd");
    }
}
