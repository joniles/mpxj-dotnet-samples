using net.sf.mpxj;
using net.sf.mpxj.reader;
using Task = net.sf.mpxj.Task;
using net.sf.mpxj.MpxjUtilities;
using net.sf.mpxj.utility;
using net.sf.mpxj.utility.clean;

/// <summary>
/// Illustrates how to anonymize a project by replacing the text it contains with nonsense.
/// Preserves the structure of the file to allow debugging MPXJ issues without
/// revealing potentially sensitive content.
/// </summary>
public class AnonymizeAProject
{
	public void Execute()
	{
        new ProjectCleanUtility().Process("sensitive-file.mpp", "clean-file.mpp");
    }
}
