using MPXJ.Net;

namespace MPXJ.Samples.HowToRead;

public class SDEFIgnoreErrors
{
    public void Read()
    {
        var reader = new SDEFReader();
        reader.IgnoreErrors = false;
        var project = reader.Read("my-sample.sdef");
    }
}
