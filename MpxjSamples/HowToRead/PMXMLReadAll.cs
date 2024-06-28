using MPXJ.Net;

namespace MPXJ.Samples.HowToRead;

public class PMXMLReadAll
{
    public void Read()
    {
        var reader = new UniversalProjectReader();
        var project = reader.ReadAll("my-sample.xml");
    }
}
