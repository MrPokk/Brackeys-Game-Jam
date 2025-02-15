using System;
using System.Threading.Tasks;


namespace Engin.Utility
{
 public static class TaskUtility
 {
  public static Task WaitSeconds(uint seconds)
  {
   return Task.Delay(TimeSpan.FromSeconds(seconds));
  }
 }
}