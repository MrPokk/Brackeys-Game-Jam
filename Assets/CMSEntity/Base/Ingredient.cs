using System;
using Engin.Utility;
using UnityEngine;
[Serializable]
public class Ingredient : CMSEntity
{
    public DataIngredient Data;

    public override void RegisterComponents(params IComponent[] components)
    {
        throw new NotImplementedException();
    }
}

[Serializable]
public struct DataIngredient : IComponent
{
    public GameObject Prefab;
}
