using System.Collections.Generic;
using System.Threading.Tasks;
public class TaskSystem
{
    private SortedSet<Task> AwaitingTasks = new SortedSet<Task>();
    private HashSet<Task> AsyncTasks = new HashSet<Task>();




    public void Init()
    {
        StartTaskAwating();
        StartTaskAsync();
    }

    private async Task StartTaskAwating()
    {
        foreach (var Element in AwaitingTasks)
        {
            Element.Start();
            Element.Wait();
        }
        StartTaskAwating().Start();
    }

    private async Task StartTaskAsync()
    {
        foreach (var Element in AsyncTasks)
        {
            Element.Start();
        }
        StartTaskAsync().Start();
    }
}
