using MPXJ.Net;

public class ConvertUniversal
{
    public void Convert(string inputFile, FileFormat format, string outputFile)
    {
        var projectFile = new UniversalProjectReader().Read(inputFile);
        new UniversalProjectWriter(format).Write(projectFile, outputFile);
    }
}
