using System.Linq;

namespace MpxjSamples.HowToUse.Pwa;

using MPXJ.Net;

public class PwaReaderExample
{
    public void Read()
    {
        // The URL for your Project Server instance
        var projectServerUrl = "https://example.sharepoint.com/sites/pwa";

        // We're assuming you have already authenticated as a user and have an access token
        var accessToken = "my-access-token-from-oauth";

        // Create a reader
        var reader = new PwaReader(projectServerUrl, accessToken);

        // Retrieve the projects available and print their details
        var projects = reader.GetProjects();
        foreach (var p in projects)
        {
            System.Console.WriteLine("ID: " + p.ProjectId + " Name: " + p.ProjectName);
        }

        // Get the ID of the first project on the list
        var projectId = projects.First().ProjectId;

        // Now read the project
        var project = reader.ReadProject(projectId);
    }
}