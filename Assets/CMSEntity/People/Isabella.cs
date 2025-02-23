using Engin.Utility;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
public class Isabella : BasePeople
{
    public Isabella()
    {
        Define<DataPeople>(out DataPeople people).Prefab = Resources.Load<GameObject>($"People/{this.GetType().Name}");
        SetTextPrefab(ref people);

        people.Type = TypePeople.Trader;
        people.TypePoison = CMS.Get<AllPotion>().Bad;
        SetData(ref people);

        ModifyDataSet();
        RegisterComponents(people);
    }
    public override BasePeople ModifyDataSet()
    {
        DataComponent.Name.text = "Isabella";

        DataComponent.Description.text = $"{DialogueList.GetRandomDialogue<Isabella>()}";
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
