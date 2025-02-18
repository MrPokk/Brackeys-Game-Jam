using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.IO;
using Engin.Utility;
using System;

public class SamplePotion : Potion, IComparable<SamplePotion>
{
    public int Priority;
    public List<EffectData> EffectsMin = new();
    public List<EffectData> EffectsMax = new();

    [ContextMenu("Generate ID")]
    public void GenerateID()
    {
        ID = GetHashCode();
    }
    public int CompareTo(SamplePotion comparePart) {  
        return Priority.CompareTo(comparePart.Priority);
    }
}
public class AllPotion : CMSEntity
{
    public List<SamplePotion> potions;
    public AllPotion()
    {
        LoadAll();
        potions.Sort();
    }
    public void LoadAll()
    {
        potions = new();
        string[] fillis = Directory.GetFiles("Assets/Resources/Potion");
        foreach (string Element in fillis) {
            if (Path.GetExtension(Element) != ".prefab") continue;
            potions.Add(Resources.Load<GameObject>($"Potion/{Path.GetFileNameWithoutExtension(Element)}").GetComponent<SamplePotion>());
        }
    }
    public SamplePotion GetByID(int ID)
    {
        return potions.FirstOrDefault(x => x.ID == ID);
    }
    //Не реализован поск зелья по эффектам
    public SamplePotion GetAtEffects(List<EffectData> atEffects)
    {
        foreach (SamplePotion potion in potions)
        {
            if (TestPotions(potion, atEffects)) {
                return potion;
            }
        }
        return null;
    }
    private bool TestPotions(SamplePotion potion, List<EffectData> atEffects)
    {
        foreach (EffectData min in potion.EffectsMin) {
            EffectData effect = atEffects.FirstOrDefault(x => x.Type == min.Type);
            if (!EffectData.Accordance(effect, min)) return false;
        }
        foreach (EffectData max in potion.EffectsMax) {
            EffectData effect = atEffects.FirstOrDefault(x => x.Type == max.Type);
            if (effect == null) continue;
            if (!EffectData.Accordance(effect, max, true)) return false;
        }
        return true;
    }

    public override void RegisterComponents(params IComponent[] components)
    {
        throw new System.NotImplementedException();
    }
}