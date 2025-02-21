using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using Engin.Utility;
using System;
using Random = UnityEngine.Random;

public class SamplePotion : Potion, IComparable<SamplePotion>
{
    public int Priority;
    public Difity Difity;
    public int Level;
    public List<EffectRange> Recipe = new();

    [ContextMenu("Generate ID")]
    public void GenerateID()
    {
        ID = GetHashCode();
    }
    public int CompareTo(SamplePotion comparePart) {  
        return Priority - comparePart.Priority;
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
    public SamplePotion GetByIDRandom(IEnumerable<int> ID)
    {
        return GetByID(ID.ElementAt(Random.Range(0, ID.Count())));
    }

    public SamplePotion GetAtEffects(List<EffectData> atEffects, SamplePotion priority = null)
    {
        if (priority != null) {
            if (CheckPotions(priority, atEffects)) return priority;
        }
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
        foreach (EffectRange effectRange in potion.Recipe) {
            EffectData effect = atEffects.FirstOrDefault(x => x.Type == effectRange.Type);
            int power;
            if (effect == null) power = 0;
            else power = effect.Power;
            if (power < effectRange.Min || power > effectRange.Max) return false;
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
[Serializable]
public class EffectRange
{
    public EffectType Type;
    [Range(-20, 20)]
    public int Min;
    [Range(-20, 20)]
    public int Max;
}
public enum Difity
{
    VeryEasy = 1,
    Easy,
    Normal,
    Hard,
    VeryHard
}