using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using Engin.Utility;
using System;
using Random = UnityEngine.Random;
using System.Xml.Linq;

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
    public List<SamplePotion> Potions;
    public SamplePotion Bad;
    public AllPotion()
    {
        LoadAll();
        Potions.Sort();
    }
    public void LoadAll()
    {
        Potions = new();
        Bad = Resources.Load<GameObject>($"Potion/Bad").GetComponent<SamplePotion>();
        string[] fillis = Directory.GetFiles("Assets/Resources/Potion");
        foreach (string Element in fillis) {
            if (Path.GetExtension(Element) != ".prefab") continue;
            Potions.Add(Resources.Load<GameObject>($"Potion/{Path.GetFileNameWithoutExtension(Element)}").GetComponent<SamplePotion>());
        }
        Potions.Remove(Bad);
    }
    public SamplePotion GetByID(int ID)
    {
        SamplePotion potion = Potions.FirstOrDefault(x => x.ID == ID);
        if (potion != null) return potion;
        if (ID == Bad.ID) return Bad;
        return null;
    }

    public SamplePotion GetAtEffects(List<EffectData> atEffects)
    {
        foreach (SamplePotion potion in Potions)
        {
            if (CheckPotions(potion, atEffects)) {
                return potion;
            }
        }
        return Bad;
    }
    private bool CheckPotions(SamplePotion potion, List<EffectData> atEffects)
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
    public SamplePotion GetRandom()
    {
        return Potions[Random.Range(0, Potions.Count)];
    }

    public override void RegisterComponents(params IComponent[] components)
    {
        throw new System.NotImplementedException();
    }
}