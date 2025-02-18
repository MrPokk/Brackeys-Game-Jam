using Engin.Utility;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
public class Dragon : BasePeople
{
    public Dragon()
    {
        Define<DataPeople>(out DataPeople people).Prefab = PrefabUtility.LoadPrefabContents("Assets/Prefab/People/Dragon.prefab");
        SetTextPrefab(ref people);
        
        people.Name.text = "Dragon";
        people.Description.text = "I, Mr. Dragon, want healing potions.";

        SetData(ref people);
        RegisterComponents(people);
    }
    public override void RegisterComponents(params IComponent[] components)
    {
        Components.AddRange(components);
    }
    public override void SetData(ref DataPeople Data)
    {
        DataComponent = Data;
    }
}
