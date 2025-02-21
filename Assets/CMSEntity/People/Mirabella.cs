using Engin.Utility;
using UnityEngine;
public class Mirabella : BasePeople
{
    Mirabella()
    {
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
        DataComponent.Name.text = "Mirabella";

        DataComponent.TypePoison = CMS.Get<AllPotion>().GetByIDRandom(new []{27668,37276,37640});
        DataComponent.Description.text = $"{DialogueList.GetRandomDialogue<Mirabella>()}";
        
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
