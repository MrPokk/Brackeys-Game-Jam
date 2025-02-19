using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Engin.Utility;
using TMPro;
using UnityEditor;
using UnityEngine;
[Serializable]
public class Ingredient : MonoBehaviour, IRaise
{
    public int ID  ;
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
    public List<ObjectIngredient> Prefabs;
    public AllIngredients()
    {
        LoadAll();
    }
    public void LoadAll()
    {
        Prefabs = new();
        string[] fillis = Directory.GetFiles("Assets/Resources/Ingredient");
        foreach (string Element in fillis) {
            if (Path.GetExtension(Element) != ".prefab") continue;
            Prefabs.Add(new ObjectIngredient(Resources.Load<GameObject>($"Ingredient/{Path.GetFileNameWithoutExtension(Element)}")));
        }
    }
    public Ingredient GetByID(int ID)
    {
        return Prefabs.FirstOrDefault(x => x.Ingredient.ID == ID).Ingredient;
    }

    public override void RegisterComponents(params IComponent[] components)
    {
        throw new NotImplementedException();
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