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
        "You can craft potions by adding the necessary ingredients to the cauldron.",
        "These <color=#e7bb2a>panels display </color>information about the <color=#4080FF> client's potions</color> and the <color=#ff40FF>potions in your cauldron.</color>",
        "To create a potion, it is necessary to match the <color=#e7bb2a>effects.</color>",
        "When you click next <color=#4080FF>client.</color>",
        "You will <color=#22f814>win</color> if the reputation is 100."
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
