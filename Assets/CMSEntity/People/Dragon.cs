using Engin.Utility;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
public class Dragon : BasePeople
{
    public Dragon()
    {
        Define<DataPeople>(out DataPeople people).Prefab = Resources.Load<GameObject>("People/Dragon");
        SetTextPrefab(ref people);
        
        people.Description.text = "Dragon";
        people.Name.text = "I, Mr. Dragon, want healing potions.";
        
        Data = people;
        
        RegisterComponents(people);
    }
    public override void RegisterComponents(params IComponent[] components)
    {
        Components.AddRange(components);
    }
}
