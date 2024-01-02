using net.sf.mpxj.reader;
using net.sf.mpxj.writer;

namespace MpxjSamples.HowToConvert;

public class ConvertUniversal
{
	public void Convert(string inputFile, string outputFile)
	{
        var reader = new UniversalProjectReader();
        var projectFile = reader.read(inputFile);

        var writer = ProjectWriterUtility.getProjectWriter(outputFile);
        writer.write(projectFile, outputFile);
    }
}
