using MPXJ.Net;

namespace MpxjSamples.HowToRead;

public class OpenPlan
{
    public void Read()
    {
        var reader = new UniversalProjectReader();
        var project = reader.Read("my-sample.bk3");
    }
}
