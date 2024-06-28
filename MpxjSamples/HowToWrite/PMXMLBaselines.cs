using MPXJ.Net;

namespace MPXJ.Samples.HowToWrite;

public class PMXMLBaselines
{
    public void Write(ProjectFile project, string fileName)
    {
        var writer = new PrimaveraPMFileWriter();
        writer.WriteBaselines = true;
        writer.Write(project, fileName);
    }
}
