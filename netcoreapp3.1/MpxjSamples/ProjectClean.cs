using net.sf.mpxj.utility;

public class ProjectClean
{
    public void Execute()
    {
        //
        // Anonymize a file allowing otherwise sensitive schedules to be shared
        // for support with MPXJ issues.
        //
        new ProjectCleanUtility().Process("/Users/joniles/Downloads/three-tasks.mpp", "/Users/joniles/Downloads/test.mpp");
    }
}


