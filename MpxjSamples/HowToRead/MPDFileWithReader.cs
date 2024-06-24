using MPXJ.Net;

public class MPDFileWithReader
{
    public void Read()
    {
        var reader = new MPDFileReader();

        // Retrieve the project details
        var projects = reader.ListProjects("my-sample.mpd");

        // Look up the project we want to read from the map.
        // For this example we'll just use a hard-coded value.
        var projectID = 1;

        // Set the ID f the project we want to read
        reader.ProjectID = projectID;

        // Read the project
        var project = reader.Read("my-sample.mpd");
    }
}

