using MPXJ.Net;

namespace MpxjSamples.HowToRead;

public class Planner
{
    public void Read()
    {
        var reader = new UniversalProjectReader();
        var project = reader.Read("my-samplexml");
    }
}
