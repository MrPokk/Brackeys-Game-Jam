using Engin.Utility;

public static class GameData<T> where T : IMain
{
    public static T Boot;
    public static bool IsStartGame;


    public const float MAX_REPUTATION = 100;

    public static float Reputation
    {
        get {
            return _Reputation;
        }
        set {
            if (_Reputation > 0) {
                _Reputation = value;
                foreach (var Element in InteractionCache<GameDataInfo>.AllInteraction) {
                    Element.UpdateReputation(value);
                }
            }
            if (_Reputation >= MAX_REPUTATION) {
                GameDataInfo.WinGame();
            }
            else if (_Reputation <= 0) {
                GameDataInfo.LoseGame();
            }
        }
    }
    private static float _Reputation = 20;
    public static int Money
    {
        get {
            return _Money;
        }
        set {
            _Money = value;

            foreach (var Element in InteractionCache<GameDataInfo>.AllInteraction) {
                Element.UpdateMoney(value);
            }
        }
    }
    private static int _Money = 100;

}
