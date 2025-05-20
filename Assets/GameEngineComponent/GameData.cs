using Engin.Utility;
using UnityEngine;

public static class GameData<T> where T : IMain
{
    public static T Boot;
    public static bool IsStartGame;


    public const float MAX_REPUTATION = 100;
    public static bool Win;

    public static int Reputation
    {
        get {
            return _Reputation;
        }
        set {
            foreach (var Element in InteractionCache<GameDataInfo>.AllInteraction) {
                Element.UpdateReputation(value - _Reputation);
            }
            _Reputation = value;
        }
    }
    private static int _Reputation = 0;
    public static int Money
    {
        get {
            return _Money;
        }
        set {
            foreach (var Element in InteractionCache<GameDataInfo>.AllInteraction) {
                Element.UpdateMoney(value - _Money);
            }
            _Money = value;
        }
    }
    private static int _Money = 20;

}
