using DG.Tweening;
using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PeopleImplementation : BaseInteraction, IEnterInPeople
{
    private const int CountViaTrader = 3;
    private int NexstTrader = CountViaTrader;
    public GameObject CustomerInGame { get; private set; }
    public static BasePeople Customer { get; private set; } = null;
    public bool IsServiced { get; private set; } = false;
    public static void ExitAll()
    {
        for (int i = 0; i < InteractionCache<PeopleImplementation>.AllInteraction.Count(); i++)
        {
            GameData<Main>.Boot.GetComponent<MonoBehaviour>().StartCoroutine(InteractionCache<PeopleImplementation>.AllInteraction[i].Exit());
        }
    }
    public IEnumerator Enter()
    {
        if (Customer == null && !IsServiced)
        {
            if (NexstTrader == 0)
            {
                Customer = PeopleMaster.GetRandTrader();
                NexstTrader = CountViaTrader;
            }
            else
            {
                Customer = PeopleMaster.GetRandCustomer();
                NexstTrader--;
            }
            Customer.ModifyDataSet();

            #if UNITY_EDITOR
            Customer.DataComponent.TypePoison = MyDebug.CustomCustomerPotion(123);
  #endif

            yield return new WaitForSeconds(1f);
            
            CustomerInGame = GameData<Main>.Boot.AddCustomer(Customer);
            var Popup = CustomerInGame.transform.Find("Popup").gameObject;
            Main.TogglePopup(Popup);

            yield return CustomerInGame.transform.DOMove(GameData<Main>.Boot.PointEndPeople.position, Main.AnimationMoveTime + 1f).SetEase(Ease.OutCirc).WaitForCompletion();

            yield return new WaitForSeconds(.5f);

            Main.TogglePopup(Popup);

            if (CustomerInGame != null)
                CustomerInGame.transform.DOComplete();
           
            SoundManager.PlaySound(SoundType.OpenPopup);
            PotionInfo.OpenPopup<PeopleImplementation>();

            IsServiced = true;

            foreach (var Element in InteractionCache<PotionInfo>.AllInteraction)
                Element.UpdateInfo();
        }
        yield break;
    }
    public IEnumerator Exit()
    {
        var Popup = CustomerInGame.transform.Find("Popup").gameObject;
        Main.TogglePopup(Popup);

        yield return CustomerInGame.transform.DOMove(GameData<Main>.Boot.PointStartPeople.position, Main.AnimationMoveTime).SetEase(Ease.InCirc).WaitForCompletion();

        CustomerInGame.transform.DOComplete();

        if (CustomerInGame != null && Customer != null)
        {
            GameData<Main>.Boot.DeleteCustomer(CustomerInGame);
            Customer = null;
            IsServiced = false;
        }

        PotionInfo.ClosePopup<PeopleImplementation>();

        GameData<Main>.Boot.GetComponent<MonoBehaviour>().StartCoroutine(Enter());
        yield break;
    }
}

public static class PeopleMaster
{
    private static List<BasePeople> customer = new();
    private static List<BasePeople> trader = new();

    private static List<BasePeople> pullCustomer = new();
    private static List<BasePeople> pullTrader = new();
    public static BasePeople GetRandCustomer() => GetRand(pullCustomer, customer);
    public static BasePeople GetRandTrader() => GetRand(pullTrader, trader);
    public static BasePeople GetRand(List<BasePeople> peoples, List<BasePeople> parent)
    {
        BasePeople people = peoples[Random.Range(0, peoples.Count)];
        peoples.Remove(people);
        if (peoples.Count == 0)
        {
            peoples.AddRange(parent);
        }
        return people;
    }
    public static void Load()
    {
        List<BasePeople> allPeoples = CMS.GetAll<BasePeople>();
        foreach (var people in allPeoples)
        {
            if (people.DataComponent.Type == TypePeople.Customer)
            {
                customer.Add(people);
            }
            else if (people.DataComponent.Type == TypePeople.Trader)
            {
                trader.Add(people);
            }
        }
        pullCustomer.AddRange(customer);
        pullTrader.AddRange(trader);
    }
}
