using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Engin.Utility;
using TMPro;
using UnityEngine;

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
        return ID.CompareTo(comparePart.ID);
    }
}

public class AllIngredients : CMSEntity
{
    public List<Ingredient> Ingredients;
    public AllIngredients()
    {
        LoadAll();
    }
    public void LoadAll()
    {
        Ingredients = new();
        string[] fillis = Directory.GetFiles("Assets/Resources/Ingredient");
        foreach (string Element in fillis) {
            if (Path.GetExtension(Element) != ".prefab") continue;
            Ingredients.Add(Resources.Load<GameObject>($"Ingredient/{Path.GetFileNameWithoutExtension(Element)}").GetComponent<Ingredient>());
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