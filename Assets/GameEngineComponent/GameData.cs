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

   foreach (var Element in  InteractionCache<GameDataInfo>.AllInteraction)
   {
    Element.Update();
   }
  }
 }
 private static int _Money = 100;
 
}
