using Engin.Utility;

public static class GameData<T> where T : IMain
{
 public static T Boot;
 public static bool IsStartGame;
 
 public static int Reputation;
 
 public static int Money {
  get {
   return _Money;
  }
  set {
   _Money = value;
  }
 }
 private static int _Money;
 
}
