using MPXJ.Net;

namespace MPXJ.Samples.HowToRead;

public class MPDFile
{
    public void Read()
    {
        var reader = new UniversalProjectReader();
        var project = reader.Read("my-sample.mpd");
    }
}
