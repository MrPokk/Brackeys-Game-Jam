using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

class GameDataInfo : BaseInteraction, IUpdateGameData
{
    public void LoadGameData()
    {
        GameData<Main>.Boot.TextManager.Get("Money").SetText(GameData<Main>.Money.ToString());
        GameData<Main>.Boot.TextManager.Get("Reputation").SetText(GameData<Main>.Reputation.ToString("0.0"));

        GameData<Main>.Boot.TextManager.Get("Reputation").text += $" / {GameData<Main>.MAX_REPUTATION}";
    }
    public bool EndMove = true;
    public void UpdateMoney(int delta = 0)
    {

        var BaseMoney = GameData<Main>.Boot.TextManager.Get("Money");

        var PlusMoney = GameData<Main>.Boot.TextManager.Get("Money Plus");

        PlusMoney.SetText(delta.ToString());
        if (delta > 0) PlusMoney.color = Color.green;
        else PlusMoney.color = Color.red;

        var BasePoseMoney = PlusMoney.transform.position;

        if (EndMove) {
            EndMove = false;
            PlusMoney.gameObject.SetActive(true);
            PlusMoney.transform.DOMove(BaseMoney.transform.position, Main.AnimationMoveTime * 2).OnComplete(() => {
                BaseMoney.SetText(GameData<Main>.Money.ToString());
                PlusMoney.gameObject.SetActive(false);

                PlusMoney.transform.position = BasePoseMoney;
                EndMove = true;
            }).SetEase(Ease.InOutElastic);
        }
    }
    public void UpdateReputation(float delta = 0)
    {
        var BaseReputation = GameData<Main>.Boot.TextManager.Get("Reputation");

        var PlusReputation = GameData<Main>.Boot.TextManager.Get("Reputation Plus");

        PlusReputation.SetText(delta.ToString("0.0"));
        if (delta > 0) PlusReputation.color = Color.green;
        else PlusReputation.color = Color.red;
        var BasePoseReputation = PlusReputation.transform.position;


        PlusReputation.gameObject.SetActive(true);
        PlusReputation.transform.DOMove(BaseReputation.transform.position, Main.AnimationMoveTime * 2).OnComplete(() => {
            BaseReputation.SetText(GameData<Main>.Reputation.ToString("0.0"));
            BaseReputation.text += $" / {GameData<Main>.MAX_REPUTATION}";

            PlusReputation.gameObject.SetActive(false);

            PlusReputation.transform.position = BasePoseReputation;
            EndMove = true;
        }).SetEase(Ease.InOutElastic);
    }
    public static void LoseGame()
    {
        GameData<Main>.Boot.LosePopup.SetActive(true);

        FileWriter.WriteLoss();
    }
    public static void WinGame()
    {
        GameData<Main>.Boot.WinPopup.SetActive(true);
        FileWriter.WriteWin();
    }
}