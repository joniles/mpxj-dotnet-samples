using MPXJ.Net;

namespace MPXJ.Samples.HowToRead;

public class SureTrakDirectory
{
    public void Read()
    {
        var reader = new UniversalProjectReader();
        var project = reader.Read("my-suretrak-directory");
    }
}
