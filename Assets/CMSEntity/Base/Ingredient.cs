using System;
using System.Collections.Generic;
using System.Linq;
using Engin.Utility;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class Ingredient : Raise , IComparable<Ingredient>
{
    public int ID;
    public String Name;
    public String Description;
    public int Price;
    public List<EffectData> Effects = new();

    public TMP_Text PriceText => GetComponentInChildren<TextMeshPro>(true);

    [ContextMenu("Generate ID")]
    public void GenerateID()
    {
        ID = GetHashCode();
    }
    
    public int CompareTo(Ingredient comparePart) {  
        return ID - comparePart.ID;
    }
}

public class AllIngredients : CMSEntity
{
    public List<Ingredient> Ingredients = new();
    public AllIngredients()
    {
        LoadAll();
    }
    public void LoadAll()
    {
        GameObject[] objects = Resources.LoadAll<GameObject>("Ingredient");
        foreach (GameObject obj in objects) {
            Ingredients.Add(obj.GetComponent<Ingredient>());
        }
    }
    public Ingredient GetByID(int ID)
    {
        return Ingredients.FirstOrDefault(x => x.ID == ID);
    }

    public override void RegisterComponents(params IComponent[] components)
    {
        throw new NotImplementedException();
    }
}