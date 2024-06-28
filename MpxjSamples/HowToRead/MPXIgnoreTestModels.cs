using MPXJ.Net;

namespace MPXJ.Samples.HowToRead;

public class MPXIgnoreTextModels
{
    public void Read()
    {
        var reader = new MPXReader();
        reader.IgnoreTextModels = false;
        var project = reader.Read("my-sample.mpx");
    }
}
