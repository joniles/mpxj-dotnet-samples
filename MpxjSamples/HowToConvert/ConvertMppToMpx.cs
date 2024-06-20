using MPXJ.Net;

public class ConvertMppToMpx
{
    public void Convert(string inputFile, string outputFile)
    {
        var reader = new MPPReader();
        var projectFile = reader.Read(inputFile);

        var writer = new MPXWriter();
        writer.Write(projectFile, outputFile);
    }
}
