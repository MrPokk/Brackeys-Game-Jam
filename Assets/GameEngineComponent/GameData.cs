using Engin.Utility;

public static class GameData<T> where T : IMain
{
 public static T Boot;
 public static bool IsStartGame;
 
 
 private const float MAX_REPUTATION = 200;
 
 public static float Reputation {
  get {
   return _Reputation;
  }
  set {
   if (_Reputation > 0)
   {
    _Reputation = value;
    foreach (var Element in  InteractionCache<GameDataInfo>.AllInteraction)
    {
     Element.Update();
    }
   }
   if (_Reputation > MAX_REPUTATION)
   {
    GameDataInfo.LoseGame();
   }
   else
   {
    GameDataInfo.LoseGame();
   }
  }
 }
 private static float _Reputation = 100;
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
