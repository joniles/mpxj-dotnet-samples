using MPXJ.Net;

public class P3NamedProject
{
    public void Read()
    {
        // Find a list of the project names
        var directory = "my-p3-directory";
        var projectNames = P3DatabaseReader.ListProjectNames(directory);

        // Tell the reader which project to work with
        var reader = new P3DatabaseReader();
        reader.ProjectName = projectNames[0];

        // Read the project
        var project = reader.Read(directory);
    }
}
