using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.IO;
using Engin.Utility;

public class SamplePotion : MonoBehaviour
{
    public int ID;
    public int Priority;
    public string Name;
    public int Price;
    public List<EffectData> EffectsMin = new();
    public List<EffectData> EffectsMax = new();

    [ContextMenu("Generate ID")]
    public void GenerateID()
    {
        ID = GetHashCode();
    }
}
public class AllPotion : CMSEntity
{
    public List<SamplePotion> potions;
    public AllPotion()
    {
        LoadAll();
    }
    public void LoadAll()
    {
        potions = new();
        string[] fillis = Directory.GetFiles("Assets/Prefab/Potion");
        foreach (string Element in fillis) {
            if (Path.GetExtension(Element) != ".prefab") continue;
            potions.Add(PrefabUtility.LoadPrefabContents(Element).GetComponent<SamplePotion>());
        }
    }
    public SamplePotion GetByID(int ID)
    {
        return potions.FirstOrDefault(x => x.ID == ID);
    }
    //Не реализован поск зелья по эффектам
    public SamplePotion GetAtEffects(List<EffectData> effects)
    {
        return null;
    }

    public override void RegisterComponents(params IComponent[] components)
    {
        throw new System.NotImplementedException();
    }
}