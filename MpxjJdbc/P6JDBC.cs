using com.microsoft.sqlserver.jdbc;
using MPXJ.Net;


namespace MpxjJdbc
{
    public class P6JDBC
    {
        public void Read()
        {
            //
            // Load the JDBC driver
            //
            var driver = new SQLServerDriver();

            //
            // Open a database connection. You will need to change
            // these details to match the name of your server, database, user and password.
            //
            var connectionString = "jdbc:sqlserver://localhost:1433;databaseName=my-database-name;user=my-user-name;password=my-password;";
            var connection = driver.connect(connectionString, null);
            var reader = new PrimaveraDatabaseReader();
            reader.Connection = connection;

            //
            // Retrieve a list of the projects available in the database
            //
            var projects = reader.ListProjects();

            //
            // At this point you'll select the project
            // you want to work with.
            //

            //
            // Now open the selected project using its ID
            //
            int selectedProjectID = 1;
            reader.ProjectID = selectedProjectID;
            var projectFile = reader.Read();
        }
    }
}
