using MPXJ.Net;

namespace MPXJ.Samples.HowToRead;

public class P6Sqlite
{
    public void Read()
    {
        var reader = new PrimaveraDatabaseFileReader();

        //
        // Retrieve a list of the projects available in the database
        //
        var file = "PPMDBSQLite.db";
        var projects = reader.ListProjects(file);

        //
        // At this point you'll select the project
        // you want to work with.
        //

        //
        // Now open the selected project using its ID
        //
        var selectedProjectId = 1;
        reader.ProjectID = selectedProjectId;
        var projectFile = reader.Read(file);
    }
}
