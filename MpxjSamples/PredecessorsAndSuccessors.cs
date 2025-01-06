using System;
using MPXJ.Net;

namespace MpxjSamples;

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
                Console.WriteLine("Task UniqueID: " + t.UniqueID + " Predecessor: " + r.PredecessorTask.UniqueID + " Type: " + r.Type);
            }
        }

        Console.WriteLine();

        foreach (var t in file.Tasks)
        {
            foreach (var r in t.Successors)
            {
                Console.WriteLine("Task UniqueID: " + t.UniqueID + " Successor: " + r.SuccessorTask.UniqueID + " Type: " + r.Type);
            }
        }
    }
}