using DG.Tweening;
using System.Collections.Generic;
using TMPro;

class TutorialInfo : BaseInteraction, IUpdateTutorialInfo
{

    private static TMP_Text TutorialText = GameData<Main>.Boot.TextManager.Get("TutorialText");
    public static bool TutorialComplete = false;
    private int TutorialStateIndex = 0;

    public static List<string> TutorialTextList = new List<string>()
    {
        "Ваша задача готовить зелья, добавляя необходимые ингредиенты в котел.",
        "На этих панелях <color=#e7bb2a> отображается </color>информация о <color=#4080FF> зельях клиента</color> и <color=#ff40FF>зельях в вашем котле.</color>",
        "Чтобы создать зелье, необходимо, чтобы оно соответствовало <color=#e7bb2a>эффектам</color>.",
        "Когда вы нажмете <color=#e7bb2a>колокольчик</color>, предоставив зелье, придёт следующий <color=#4080FF>клиент</color>.",
        "Вы выиграете <color=#22f814>, если ваша репутация будет равна 100."
    };
    public void Update()
    {
        if (TutorialStateIndex >= GameData<Main>.Boot.TutorialManager.TutorialState.Count)
        {
            GameData<Main>.Boot.TutorialManager.gameObject.transform.DOScale(0f, Main.AnimationScaleTime).OnComplete(() => GameData<Main>.Boot.TutorialManager.gameObject.SetActive(false));
            TutorialStateIndex = 0;
            return;
        }

        if (TutorialComplete) GameData<Main>.Boot.TutorialManager.gameObject.SetActive(false);

        TutorialText.text = TutorialTextList[TutorialStateIndex];
        GameData<Main>.Boot.TutorialManager.TutorialState[TutorialStateIndex].SetActive(true);

        if (TutorialStateIndex != 0)
        {
            GameData<Main>.Boot.TutorialManager.TutorialState[TutorialStateIndex - 1].SetActive(false);
        }

        TutorialStateIndex++;

    }
}
