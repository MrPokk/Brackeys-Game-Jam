using Engin.Utility;

public static class GameData<T> where T : IMain
{
 public static T Boot;
 public static bool IsStartGame;

 public static int Money;
 public static int Reputation;
 
}
