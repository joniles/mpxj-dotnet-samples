namespace MpxjSamples.HowToUse.Universal;

using MPXJ.Net;
    
public class SimpleExample
{
    public void Process()
    {
        var reader = new UniversalProjectReader();
        var project = reader.Read("example.mpp");
    }
}