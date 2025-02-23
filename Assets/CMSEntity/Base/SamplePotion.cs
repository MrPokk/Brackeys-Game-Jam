using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Engin.Utility;
using System;
using Random = UnityEngine.Random;
using Unity.VisualScripting;

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
    public Dictionary<SamplePotion, int> PotionsPull;
    public AllPotion()
    {
        Potions = new();
        PotionsPull = new();
        LoadAll();
        InitPull();
        Potions.Sort();
    }
    private void LoadAll()
    {
        GameObject[] objects = Resources.LoadAll<GameObject>("Potion");
        Bad = Resources.Load<GameObject>("Potion/Bad").GetComponent<SamplePotion>();
        foreach (GameObject obj in objects) {
            SamplePotion potion = obj.GetComponent<SamplePotion>();
            Potions.Add(potion);
        }
        Potions.Remove(Bad);
    }
    private void InitPull()
    {
        foreach (SamplePotion potion in Potions) {
            PotionsPull.Add(potion, 6 - (int)potion.Difity);
        }
    }
    public SamplePotion GetByID(int ID)
    {
        SamplePotion potion = Potions.FirstOrDefault(x => x.ID == ID);
        if (potion != null) return potion;
        if (ID == Bad.ID) return Bad;
        return null;
    }
    public SamplePotion GetByIDRandom(IEnumerable<int> IDs)
    {
        Dictionary<SamplePotion, int> random = new();
        foreach (int id in IDs) {
            SamplePotion potion = GetByID(id);
            random.Add(potion, PotionsPull[potion]);
        }
        int Sum = 0;
        foreach (int weight in random.Values) {
            Sum += weight;
        }
        int RandNum = Random.Range(0, Sum);
        foreach (var item in random) {
            RandNum -= item.Value;
            if (RandNum <= 0) {
                SamplePotion potion = item.Key;
                if ((PotionsPull[potion] -= 1) == 0) {
                    AddWeightAll();
                }
                return potion;
            }
        }
        throw new Exception("Не найдено зелье в списках весов");
    }
    public List<int> GetIDsOfDifity(Difity difity)
    {
        List<int> IDs = new();
        foreach (SamplePotion potion in Potions) {
            if (potion.Difity == difity) {
                IDs.Add(potion.ID);
            }
        }
        return IDs;
    }
    private void AddWeightAll()
    {
        Dictionary<SamplePotion, int> old = new();
        old.AddRange(PotionsPull);
        foreach (var item in old) {
            PotionsPull[item.Key] += 6 - (int)item.Key.Difity;
        }
        Debug.Log("Веса обновлены");
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