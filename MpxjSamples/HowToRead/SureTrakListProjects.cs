using MPXJ.Net;

namespace MpxjSamples.HowToRead;

public class SureTrakListProjects
{
    public void Read()
    {
        // Find a list of the project names
        var directory = "my-suretrak-directory";
        var projectNames = SureTrakDatabaseReader.ListProjectNames(directory);

        // Tell the reader which project to work with
        var reader = new SureTrakDatabaseReader();
        reader.ProjectName = projectNames[0];

        // Read the project
        var project = reader.Read(directory);
    }
}
