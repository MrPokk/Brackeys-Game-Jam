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
        
        SetData(ref people);
        
        ModifyDataSet();
        RegisterComponents(people);
    }
    public override BasePeople ModifyDataSet()
    {
        DataComponent.Name.text = "Sidorovich";
        DataComponent.Description.text = $"I, Mr. Sidorovich and i have an item for sale.";
        return this;
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
