using MPXJ.Net;

namespace MpxjSamples.HowToRead;

public class MPPWithPassword
{
    public void Read()
    {
        var reader = new MPPReader();
        reader.ReadPassword = "my secret password";
        var project = reader.Read("my-sample.mpp");
    }
}
