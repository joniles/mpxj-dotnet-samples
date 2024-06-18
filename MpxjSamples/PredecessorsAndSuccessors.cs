using MPXJ.Net;
using Task = MPXJ.Net.Task;

/// <summary>
/// Illustrates retrieving predecessor and successor details from a project.
/// </summary>
public class PredecessorsAndSuccessors
{
    public void Execute(string filename)
    {
        ProjectFile file = new UniversalProjectReader().Read(filename);

        foreach (Task t in file.Tasks)
        {
            foreach (Relation r in t.Predecessors)
            {
                System.Console.WriteLine("Task UniqueID: " + t.UniqueID + " Predcessor: " + r.TargetTask.UniqueID + " Type: " + r.Type);
            }
        }

        System.Console.WriteLine();

        foreach (Task t in file.Tasks)
        {
            foreach (Relation r in t.Successors)
            {
                System.Console.WriteLine("Task UniqueID: " + t.UniqueID + " Successor: " + r.TargetTask.UniqueID + " Type: " + r.Type);
            }
        }
    }
}
