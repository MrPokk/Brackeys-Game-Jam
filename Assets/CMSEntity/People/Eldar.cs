using Engin.Utility;
using UnityEngine;

public class Eldar : BasePeople
{
    public Eldar()
    {
        
        Define<DataPeople>(out DataPeople people).Prefab = Resources.Load<GameObject>($"People/{this.GetType().Name}");
        SetTextPrefab(ref people);

        people.Type = TypePeople.Customer;
        people.TypePoison = CMS.Get<AllPotion>().Bad;
        people.IDsPotions = new[] { 27668, 28024, 37276 };

        SetData(ref people);

        ModifyDataSet();
        RegisterComponents(people);
    }

    public override BasePeople ModifyDataSet()
    {
        DataComponent.Name.text = "Eldar";
        DataComponent.Description.text = $"{DialogueList.GetRandomDialogue<Eldar>()}";
        return this;
    }

    public string GetDialoge()
    {

        return DataComponent.TypePoison.Name;
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
