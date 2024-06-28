using MPXJ.Net;

namespace MPXJ.Samples.HowToRead;

public class P6IgnoreErrors
{
    public void Read()
    {
        var reader = new PrimaveraDatabaseReader();
        reader.IgnoreErrors = false;
    }
}
