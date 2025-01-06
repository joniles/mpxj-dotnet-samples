using MPXJ.Net;

namespace MpxjSamples.HowToRead;

public class PMXMLReadAll
{
    public void Read()
    {
        var reader = new UniversalProjectReader();
        var project = reader.ReadAll("my-sample.xml");
    }
}
