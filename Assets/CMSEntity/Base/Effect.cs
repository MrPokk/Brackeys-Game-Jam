using Engin.Utility;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public EffectType ID;
    public string Name {
        get {
            return NameEffect;
        }
    }
    [SerializeField] private string NameEffect;
    public Color Color = Color.black;
}

public class AllEffect : CMSEntity
{
    public List<Effect> effects;
    public AllEffect()
    {
        effects = new();
        LoadAll();
    }
    public void LoadAll()
    {
        GameObject[] objects = Resources.LoadAll<GameObject>($"Effect");
        foreach (GameObject obj in objects) {
            effects.Add(obj.GetComponent<Effect>());
        }
    }
    public Effect GetAtID(EffectType type)
    {
        return effects.FirstOrDefault(x => x.ID == type);
    }

    public override void RegisterComponents(params IComponent[] components)
    {
        throw new System.NotImplementedException();
    }
}
