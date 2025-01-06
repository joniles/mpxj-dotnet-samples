using MPXJ.Net;

namespace MpxjSamples.HowToRead;

public class MPXIgnoreTextModels
{
    public void Read()
    {
        var reader = new MPXReader();
        reader.IgnoreTextModels = false;
        var project = reader.Read("my-sample.mpx");
    }
}
