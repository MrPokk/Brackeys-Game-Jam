using Engin.Utility;
using UnityEngine;
public class Finch : BasePeople
{
    public Finch()
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
        DataComponent.Name.text = "Finch";

        DataComponent.TypePoison = CMS.Get<AllPotion>().GetByIDRandom(new []{37360,31352,27668});
        DataComponent.Description.text = $"{DialogueList.GetRandomDialogue<Finch>()}";
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
