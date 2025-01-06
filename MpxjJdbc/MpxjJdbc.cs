using com.microsoft.sqlserver.jdbc;
using MPXJ.Net;

namespace MpxjJdbc
{
    /// <summary>
    /// The purpose of this class is to demonstrate how you can configure a JDBC driver to work with MPXJ.Net.
    /// In this example we're working with a Primavera P6 SQL Server database, but the same approach could also be taken
    /// for an Oracle database.
    ///
    /// For this to work you need to add a reference to the JDBC driver to the csproj file, as follows:
    /// <MavenReference Include="com.microsoft.sqlserver:mssql-jdbc" Version="12.6.2.jre8" />
    /// 
    /// The utility is expecting to receive three command line arguments, a JDBC connection string, a project ID, and finally an output file name.
    /// The structure of the connection string for the Microsoft JDBC driver is defined here:
    /// https://docs.microsoft.com/en-us/sql/connect/jdbc/building-the-connection-url?view=sql-server-2017
    ///
    /// The available projectIDs can be found by calling `reader.ListProjects()`. The value is the primary key from the `project` table in
    /// the P6 database corresponding to the project you want to export.
    ///
    /// The output file name is the file into which the utility will write a PMXML file representing the data extracted from P6.
    ///
    /// Here's an example of the arguments you might pass to this utility:
    ///
    /// jdbc:sqlserver://myhostname;user=myusername;password=mypassword;databaseName=mydatabase;encrypt=false 1234 project1.xml
    /// </summary>
    public class MpxjJdbc
    {
        public static void Main(string[] args)
        {
            try
            {
                if (args.Length != 3)
                {
                    Console.Out.WriteLine("Usage: MpxjJdbc <JDBC connection string> <project ID> <output file name>");
                }
                else
                {
                    var convert = new MpxjJdbc();
                    convert.Process(args[0], Convert.ToInt32(args[1]), FileFormat.PMXML, args[2]);
                }
            }

            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.ToString());
            }
        }


        public void Process(string connectionString, int projectId, FileFormat outputFormat, string outputFile)
        {
            var driver = new SQLServerDriver();
            var connection = driver.connect(connectionString, null);

            //
            // Configure the reader
            //
            var reader = new PrimaveraDatabaseReader();
            reader.Connection = connection;
            reader.ProjectID = projectId;

            Console.Out.WriteLine("Reading from database started.");
            var start = DateTime.Now;
            var projectFile = reader.Read();
            var elapsed = DateTime.Now - start;
            Console.Out.WriteLine("Reading from database completed in " + elapsed.TotalMilliseconds + "ms.");

            Console.Out.WriteLine("Writing output file started.");
            start = DateTime.Now;
            new UniversalProjectWriter(outputFormat).Write(projectFile, outputFile);
            elapsed = DateTime.Now - start;
            Console.Out.WriteLine("Writing output completed in " + elapsed.TotalMilliseconds + "ms.");
        }
    }
}
