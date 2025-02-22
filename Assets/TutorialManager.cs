using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    public static float DELAY_TEXT = 0.1f;

    public TMP_Text TutorialText;

    private static int StateText = 0;

    public void Update()
    {
        if (Input.GetMouseButtonDown((int)MouseButton.Left))
        {
            StartCoroutine(TutorialManagerService.TextPrint(TutorialText, TutorialManagerTextList.TutorialTextList[StateText]));

            if (StateText <= TutorialManagerTextList.TutorialTextList.Count)
                StateText++;
        }
    }


    public IEnumerator StateTutorial()
    {
        yield return new WaitForSeconds(DELAY_TEXT);
    }
}

public static class TutorialManagerTextList
{
    public static List<string> TutorialTextList = new List<string>()
    {
        "Add the ingredient to the caldron",
        "Click on the potion crafting button",
        "Put the potions in the zone and tap on bell",
    };
}


public static class TutorialManagerService
{

    private static bool isPrint = false;
    public static bool Skip = false;

    public static IEnumerator TextPrint(TMP_Text output, string input)
    {
        if (isPrint) yield break;
        isPrint = true;

        for (int i = 1; i <= input.Length; i++)
        {
            if (Skip)
            {
                output.text = input;
                yield return null;
            }
            output.text = input.Substring(1, i - 1);
            yield return new WaitForSeconds(TutorialManager.DELAY_TEXT);
        }
        isPrint = false;
    }

}
