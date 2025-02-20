using Engin.Utility;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public EffectType ID;
    public string Name;
    public string NegativeName;
    public Color Color = Color.black;
}

public class AllEffect : CMSEntity
{
    public List<Effect> effects;
    public AllEffect()
    {
        LoadAll();
    }
    public void LoadAll()
    {
        effects = new();
        string[] fillis = Directory.GetFiles("Assets/Resources/Effect");
        foreach (string Element in fillis) {
            if (Path.GetExtension(Element) != ".prefab") continue;
            effects.Add(Resources.Load<GameObject>($"Effect/{Path.GetFileNameWithoutExtension(Element)}").GetComponent<Effect>());
        }
    }
    public Effect GetAtID(EffectType type)
    {
        return effects.FirstOrDefault(x =>  x.ID == type);
    }

    public override void RegisterComponents(params IComponent[] components)
    {
        throw new System.NotImplementedException();
    }
}