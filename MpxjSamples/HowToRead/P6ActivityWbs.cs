using MPXJ.Net;

namespace MpxjSamples.HowToRead;

public class P6ActivityWbs
{
    public void Read()
    {
        var reader = new PrimaveraDatabaseReader();
        reader.MatchPrimaveraWBS = false;
    }
}
