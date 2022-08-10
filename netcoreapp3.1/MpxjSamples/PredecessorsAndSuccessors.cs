using net.sf.mpxj;
using net.sf.mpxj.MpxjUtilities;
using net.sf.mpxj.reader;
using Task = net.sf.mpxj.Task;

/// <summary>
/// Illustrates retrieving predecessor and successor details from a project.
/// </summary>
public class PredecessorsAndSuccessors
{
    public void Execute()
    {
        ProjectFile file = new UniversalProjectReader().read("example.mpp");

        foreach (Task t in file.Tasks)
        {
            foreach (Relation r in t.Predecessors.ToIEnumerable())
            {

                System.Console.WriteLine("Task UniqueID: " + t.UniqueID + " Predcessor: " + r.TargetTask.UniqueID + " Type: " + r.Type);
            }
        }

        System.Console.WriteLine();

        foreach (Task t in file.Tasks)
        {
            foreach (Relation r in t.Successors.ToIEnumerable())
            {
                System.Console.WriteLine("Task UniqueID: " + t.UniqueID + " Successor: " + r.TargetTask.UniqueID + " Type: " + r.Type);
            }
        }
    }
}
