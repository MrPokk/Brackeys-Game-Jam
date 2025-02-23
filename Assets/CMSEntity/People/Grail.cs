using Engin.Utility;
using UnityEngine;
public class Grail : BasePeople
{
    public Grail()
    {
        Define<DataPeople>(out DataPeople people).Prefab = Resources.Load<GameObject>($"People/{this.GetType().Name}");
        SetTextPrefab(ref people);

        people.Type = TypePeople.Customer;
        people.TypePoison = CMS.Get<AllPotion>().Bad;
        people.IDsPotions = new[] { 28988, 31260, 37360 };
        SetData(ref people);
        
        ModifyDataSet();
        RegisterComponents(people);
    }

    public override BasePeople ModifyDataSet()
    {
        DataComponent.Name.text = "Grail";
        DataComponent.Description.text = $"{DialogueList.GetRandomDialogue<Grail>()}";
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
