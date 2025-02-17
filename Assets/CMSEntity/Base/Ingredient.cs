using System;
using System.Collections.Generic;
using System.IO;
using Engin.Utility;
using TMPro;
using UnityEditor;
using UnityEngine;
[Serializable]
public class Ingredient : MonoBehaviour, IRaise
{
    public String Name;
    public String Description;
    public List<EffectData> Effects = new();
}

public class DataIngredients : CMSEntity
{
    public List<ObjectIngredient> prefabs;
    public DataIngredients()
    {
        LoadAll();
    }
    public override void RegisterComponents(params IComponent[] components)
    {
        throw new NotImplementedException();
    }
    public void LoadAll()
    {
        prefabs = new();
        string[] fillis = Directory.GetFiles("Assets/Prefab/Ingredient");
        foreach (string Element in fillis) {
            if (Path.GetExtension(Element) != ".prefab") continue;
            prefabs.Add(new ObjectIngredient(PrefabUtility.LoadPrefabContents(Element)));
        }
    }
}
[Serializable]
public class ObjectIngredient
{
    public GameObject Prefab;
    public Ingredient Ingredient;

    public ObjectIngredient(GameObject prefab)
    {
        Prefab = prefab;
        Ingredient = prefab.GetComponent<Ingredient>();
        
        if(Ingredient != null)
         SetTextPrefab(Ingredient);
    }
    private void SetTextPrefab(Ingredient ingredient)
    {
        foreach(var Element in Ingredient.GetComponentsInChildren<TMP_Text>())
        {
            if(Element.name == "Name")
            {
                Element.text = ingredient.Name;
            }
            else if (Element.name == "Description")
            {
                Element.text = ingredient.Description;
            }
         
        }
    }
}