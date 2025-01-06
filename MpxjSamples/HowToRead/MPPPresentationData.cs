using MPXJ.Net;

namespace MpxjSamples.HowToRead;

public class MPPPresentationData
{
    public void Read()
    {
        var reader = new MPPReader();
        reader.ReadPresentationData = false;
        var project = reader.Read("my-sample.mpp");
    }
}
