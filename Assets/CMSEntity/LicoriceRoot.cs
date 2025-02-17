using Engin.Utility;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

public class LicoriceRoot : Ingredient
{
    public LicoriceRoot()
    {
        Define<DataIngredient>(out var component).Prefab = PrefabUtility.LoadPrefabContents("Assets/Prefab/LicoriceRoot.prefab");
        RegisterComponents(component);
    }

    public override void RegisterComponents(params IComponent[] components)
    {
        Components.AddRange(components);
    }
}