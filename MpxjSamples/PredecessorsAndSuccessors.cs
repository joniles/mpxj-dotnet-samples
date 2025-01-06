using MPXJ.Net;

/// <summary>
/// Illustrates retrieving predecessor and successor details from a project.
/// </summary>
public class PredecessorsAndSuccessors
{
    public void Execute(string filename)
    {
        var file = new UniversalProjectReader().Read(filename);

        foreach (var t in file.Tasks)
        {
            foreach (var r in t.Predecessors)
            {
                System.Console.WriteLine("Task UniqueID: " + t.UniqueID + " Predcessor: " + r.PredecessorTask.UniqueID + " Type: " + r.Type);
            }
        }

        System.Console.WriteLine();

        foreach (var t in file.Tasks)
        {
            foreach (var r in t.Successors)
            {
                System.Console.WriteLine("Task UniqueID: " + t.UniqueID + " Successor: " + r.SuccessorTask.UniqueID + " Type: " + r.Type);
            }
        }
    }
}
