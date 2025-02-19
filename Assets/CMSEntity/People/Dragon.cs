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

        people.Type = TypePeople.Customer;
        people.TypePoison = CMS.Get<AllPotion>().GetRandom();
        people.Name.text = "Dragon";
        people.Description.text = $"I, Mr. Dragon, want <color=#ed2246>{people.TypePoison.name}</color> potions.";
        
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
