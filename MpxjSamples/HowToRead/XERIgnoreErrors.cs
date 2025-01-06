using MPXJ.Net;

namespace MPXJ.Samples.HowToRead;

public class XERIgnoreErrors
{
    public void Read()
    {
        var reader = new PrimaveraXERFileReader();
        reader.IgnoreErrors = false;
        var project = reader.Read("my-sample.xer");
    }
}
