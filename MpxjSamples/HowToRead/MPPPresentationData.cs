using MPXJ.Net;

namespace MPXJ.Samples.HowToRead;

public class MPPPresentationData
{
    public void Read()
    {
        var reader = new MPPReader();
        reader.ReadPresentationData = false;
        var project = reader.Read("my-sample.mpp");
    }
}
