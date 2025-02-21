using Engin.Utility;
using UnityEngine;

public class Eldar : BasePeople
{
    Eldar()
    {
        Debug.Log(GetType().Name);
        Define<DataPeople>(out DataPeople people).Prefab = Resources.Load<GameObject>($"People/{this.GetType().Name}");
        SetTextPrefab(ref people);

        people.Type = TypePeople.Customer;
        people.TypePoison = CMS.Get<AllPotion>().Bad;
        
        SetData(ref people);
        
        ModifyDataSet();
        RegisterComponents(people);
    }

    public override BasePeople ModifyDataSet()
    {
        DataComponent.Name.text = "Eldar";
        
        DataComponent.TypePoison = CMS.Get<AllPotion>().GetByID(27668);
        
        DataComponent.Description.text = $"{DialogueList.GetRandomDialogue<Eldar>()}";
        return this;
    }

    public  string GetDialoge()
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
