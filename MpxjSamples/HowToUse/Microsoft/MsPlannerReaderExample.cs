using System.Linq;

namespace MpxjSamples.HowToUse.Microsoft;

using MPXJ.Net;

public class MsPlannerReaderExample
{
    public void Read()
    {
        // The URL for your organisation's Dynamics server instance
        var dynamicsServerUrl = "https://example.api.crm11.dynamics.com";

        // We're assuming you have already authenticated as a user and have an access token
        var accessToken = "my-access-token-from-oauth";

        // Create a reader
        var reader = new MsPlannerReader(dynamicsServerUrl, accessToken);

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