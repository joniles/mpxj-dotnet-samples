using MPXJ.Net;

namespace MpxjSamples.HowToRead;

public class PMXML
{
    public void Read()
    {
        var reader = new UniversalProjectReader();
        var project = reader.Read("my-sample.xml");
    }
}
