using net.sf.mpxj.mpp;
using net.sf.mpxj.mpx;

namespace MpxjSamples.HowToConvert;

public class ConvertMppToMpx
{
    public void Convert(string inputFile, string outputFile)
    {
        var reader = new MPPReader();
        var projectFile = reader.read(inputFile);

        var writer = new MPXWriter();
        writer.write(projectFile, outputFile);
    }
}
