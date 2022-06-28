using net.sf.mpxj.utility;
using net.sf.mpxj.utility.clean;

public class ProjectClean
{
    //
    // Demonstrate anonymizing a file allowing otherwise sensitive schedules to be shared
    // for support with MPXJ issues.
    //
    public void Execute()
    {
        // Use the "redact" strategy
        new ProjectCleanUtility().Process(new CleanByRedactStrategy(), "/Users/joniles/Downloads/three-tasks.mpp", "/Users/joniles/Downloads/test.mpp");

        // Use the default "word replacement" strategy (produces more usable results)
        new ProjectCleanUtility().Process("/Users/joniles/Downloads/three-tasks.mpp", "/Users/joniles/Downloads/test.mpp");
    }
}


