using Engin.Utility;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
public class Sidorovich : BasePeople
{
    public Sidorovich()
    {
        Define<DataPeople>(out DataPeople people).Prefab = Resources.Load<GameObject>("People/Dragon");
        SetTextPrefab(ref people);

        people.Type = TypePeople.Trader;
        people.TypePoison = CMS.Get<AllPotion>().Bad;
        people.Name.text = "Sidorovich";
        people.Description.text = $"I, Mr. Sidorovich and i have an item for sale.";
        
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
