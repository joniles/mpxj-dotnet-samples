using MPXJ.Net;

namespace MPXJ.Samples.HowToRead;

public class Asta
{
    public void Read()
    {
        var reader = new UniversalProjectReader();
        var project = reader.Read("my-sample.pp");
    }
}
