using MPXJ.Net;

namespace MpxjSamples.HowToRead;

public class MPP
{
    public void Read()
    {
        var reader = new UniversalProjectReader();
        var project = reader.Read("my-sample.mpp");
    }
}
