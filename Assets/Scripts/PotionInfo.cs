using DG.Tweening;
using SmallHedge.SoundManager;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

class PotionInfo : BaseInteraction, IUpdatePotionInfo
{
    private TMP_Text NameCauldronPotion = GameData<Main>.Boot.TextManager.Get("PotionInfoName");
    private TMP_Text NameCustomerPotion = GameData<Main>.Boot.TextManager.Get("PotionInfoCustomerPotionName");

    private TMP_Text DescriptionAttributes = GameData<Main>.Boot.TextManager.Get("PotionInfoDescriptionAttributes");
    private TMP_Text DescriptionEffect = GameData<Main>.Boot.TextManager.Get("PotionInfoDescriptionEffect");
    private TMP_Text NeedCraftText = GameData<Main>.Boot.TextManager.Get("PotionInfoNeed");

    private static Vector3 SIZE_INFO_CAULDRON = GameData<Main>.Boot.PotionInfoCauldron.transform.localScale;
    private static Vector3 SIZE_INFO_CUSTOMER = GameData<Main>.Boot.PotionInfoCustomer.transform.localScale;
    public static void OpenPopup<T>()
    {
        Type WhoCalled = typeof(T);
        if (WhoCalled == typeof(Cauldron))
        {
            SoundManager.PlaySound(SoundType.OpenPopup);
            GameData<Main>.Boot.PotionInfoCauldron.SetActive(true);
            GameData<Main>.Boot.PotionInfoCauldron.transform.DOScale(SIZE_INFO_CAULDRON, Main.AnimationScaleTime);
        }
        else if (WhoCalled == typeof(PeopleImplementation) && PeopleImplementation.Customer.DataComponent.Type != TypePeople.Trader)
        {
            SoundManager.PlaySound(SoundType.OpenPopup);
            GameData<Main>.Boot.PotionInfoCustomer.SetActive(true);
            GameData<Main>.Boot.PotionInfoCustomer.transform.DOScale(SIZE_INFO_CUSTOMER, Main.AnimationScaleTime);
        }
    }
    public static void ClosePopup<T>()
    {
        Type WhoCalled = typeof(T);
        if (WhoCalled == typeof(Cauldron))
        {
            GameData<Main>.Boot.PotionInfoCauldron
                .transform.DOScale(0, Main.AnimationScaleTime).OnComplete(
                    (() => Main.TogglePopup(GameData<Main>.Boot.PotionInfoCauldron)));
        }
        else if (WhoCalled == typeof(PeopleImplementation))
        {
            GameData<Main>.Boot.PotionInfoCustomer
                .transform.DOScale(0, Main.AnimationScaleTime).OnComplete(
                    (() => Main.TogglePopup(GameData<Main>.Boot.PotionInfoCustomer)));
        }
    }


    public void UpdateInfo()
    {
        var EffectsInCauldron = GameData<Main>.Boot.Cauldron.effectsMaster.Get();

        List<EffectRange> EffectsInCustomer = new List<EffectRange>();
        if (PeopleImplementation.Customer != null && PeopleImplementation.Customer.DataComponent.Type == TypePeople.Trader) return;

        EffectsInCustomer.AddRange(PeopleImplementation.Customer.DataComponent.TypePoison.Recipe);

        var EffectsInCauldronTextAttributes = new List<string>();
        var EffectsInCauldronTextEffect = new List<string>();
        var EffectsInCraftText = new List<string>();

        var Potion = GameData<Main>.Boot.Cauldron.GetEffectPotion();


        foreach (var ElementCauldron in EffectsInCauldron)
        {

            if (PeopleImplementation.Customer != null && PeopleImplementation.Customer.DataComponent.Type != TypePeople.Trader)
            {

                var EffectCustomer = EffectsInCustomer.FirstOrDefault(range => {
                    if (range.Type == ElementCauldron.Type)
                        return true;
                    return false;
                });
                
             
                //Базовые эффекты 
                if (EffectCustomer != null && EffectCustomer.Type == ElementCauldron.Type && ElementCauldron.Type < EffectType.ADDITIONAL)
                {
                    var Color = (CMS.Get<AllEffect>().GetAtID(ElementCauldron.Type).Color);
                    Color.a = 1f;
                    var ColorHex = $"#{XColor.ToHexString(Color)}";
                    
                    string ColorEffectHex;
                    if (ElementCauldron.Power <= EffectCustomer.Max && ElementCauldron.Power >= EffectCustomer.Min)
                    {
                        ColorEffectHex = "#22f814";
                        SoundManager.PlaySound(SoundType.CheckingPotion);
                    }
                    else
                    {
                        ColorEffectHex = "#de1111";
                    }

                    EffectsInCauldronTextEffect.Add($"<color={ColorHex}>{ElementCauldron.Type.ToString().ToUpperInvariant()}</color>: <color={ColorEffectHex}>{ElementCauldron.Power}</color>");
                }
                else if (ElementCauldron.Type < EffectType.ADDITIONAL)
                {
                    var ColorBad = $"#373737";
                    EffectsInCauldronTextEffect.Add($"<color={ColorBad}>{ElementCauldron.Type.ToString().ToUpperInvariant()}: {ElementCauldron.Power}</color>");
                }
                
                //Дополнительные эффекты 
                if (EffectCustomer != null && EffectCustomer.Type == ElementCauldron.Type && ElementCauldron.Type > EffectType.ADDITIONAL)
                {
                    var Color = (CMS.Get<AllEffect>().GetAtID(ElementCauldron.Type).Color);
                    Color.a = 1f;
                    var ColorHex = $"#{XColor.ToHexString(Color)}";
                    
                    string ColorEffectHex;
                    if (ElementCauldron.Power <= EffectCustomer.Max && ElementCauldron.Power >= EffectCustomer.Min)
                    {
                        ColorEffectHex = "#22f814";
                        SoundManager.PlaySound(SoundType.CheckingPotion);
                    }
                    else
                    {
                        ColorEffectHex = "#de1111";
                    }

                    EffectsInCauldronTextAttributes.Add($"<color={ColorHex}>{ElementCauldron.Type.ToString().ToUpperInvariant()}</color>: <color={ColorEffectHex}>{ElementCauldron.Power}</color>");
                }
                else if (ElementCauldron.Type > EffectType.ADDITIONAL)
                {
                    var ColorBad = $"#373737";
                    EffectsInCauldronTextAttributes.Add($"<color={ColorBad}>{ElementCauldron.Type.ToString().ToUpperInvariant()}: {ElementCauldron.Power}</color>");
                }
            }
            else
            {
                EffectsInCauldronTextEffect.Add($"{ElementCauldron.Type.ToString().ToUpperInvariant()}: {ElementCauldron.Power}");
            }

        }

        if (PeopleImplementation.Customer != null)
        {
            foreach (var Element in EffectsInCustomer)
            {
                var Color = (CMS.Get<AllEffect>().GetAtID(Element.Type).Color);
                Color.a = 1f;
                var ColorHex = $"#{XColor.ToHexString(Color)}";
                EffectsInCraftText.Add($"<color={ColorHex}>{Element.Type.ToString().ToUpperInvariant()}:</color> {Element.Min} <color=#595959>to</color> {Element.Max}");
            }
        }

        if (PeopleImplementation.Customer.DataComponent.TypePoison.ID == Potion.ID)
        {
            NameCauldronPotion.SetText($"<color=#22f814>{Potion.Name}</color>");
            NameCustomerPotion.SetText($"<color=#22f814>{PeopleImplementation.Customer.DataComponent.TypePoison.Name}</color>");

            //  NameCauldronPotion.gameObject.transform.DOShakeScale(1f, Main.AnimationScaleTime,0,0).OnComplete(() => {
            //    NameCustomerPotion.gameObject.transform.DOShakeScale(1f, Main.AnimationScaleTime,0,0).SetLoops( -1, LoopType.Yoyo );
            // });

        }
        else
        {
            NameCauldronPotion.SetText($"<color=#de1111>{Potion.Name}</color>");
            NameCustomerPotion.SetText($"<color=#de1111>{PeopleImplementation.Customer.DataComponent.TypePoison.Name}</color>");


            //   NameCauldronPotion.gameObject.transform.DOShakeScale(1f, Main.AnimationScaleTime,0,0).OnComplete(() => {
            //     NameCustomerPotion.gameObject.transform.DOShakeScale(1f, Main.AnimationScaleTime,0,0).SetLoops( -1, LoopType.Yoyo );
            //});

        }

        DescriptionAttributes.SetText(string.Join("\n", EffectsInCauldronTextAttributes));
        DescriptionEffect.SetText(string.Join("\n", EffectsInCauldronTextEffect));

        NeedCraftText.SetText(string.Join("\n", EffectsInCraftText));
    }
}
