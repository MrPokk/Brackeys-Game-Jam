using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Engin.Utility;
using UnityEngine;

[Serializable]
public class Ingredient : Raise
{
    public int ID;
    public String Name;
    public String Description;
    public List<EffectData> Effects = new();

    [ContextMenu("Generate ID")]
    public void GenerateID()
    {
        ID = GetHashCode();
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