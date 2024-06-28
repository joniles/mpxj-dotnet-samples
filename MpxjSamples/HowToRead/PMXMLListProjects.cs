using MPXJ.Net;

namespace MPXJ.Samples.HowToRead;

public class PMXMLListProjects
{
    public void Read()
    {
        var reader = new PrimaveraPMFileReader();
        var stream = new FileStream("my-sample.xml",
            FileMode.Open, FileAccess.Read, FileShare.None);
        var projects = reader.ListProjects(stream);
        System.Console.WriteLine("ID\tName");
        foreach (var entry in projects)
        {
            System.Console.WriteLine($"{entry.Key}\t{entry.Value}");
        }
    }
}
