using DG.Tweening;
using SmallHedge.SoundManager;
using System;
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
    public static bool IsServiced { get; private set; } = false;
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
                PeopleMaster.GeneratePullOfDifity((Difity)(GameData<Main>.Reputation / 20), (Difity)((GameData<Main>.Reputation + 10) / 20), CountViaTrader);
            }
            else
            {
                Customer = PeopleMaster.GetRandCustomer();
                NexstTrader--;
            }
            Customer.ModifyDataSet();

            #if UNITY_EDITOR
            //Customer.DataComponent.TypePoison = MyDebug.CustomCustomerPotion(37836);
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

            if (Customer.DataComponent.Type == TypePeople.Trader) {
                GameData<Main>.Boot.Shop.Generatre(10);
            }
            
        }
        yield break;
    }
    public IEnumerator Exit()
    {
        if (CustomerInGame == null) yield break;
        var Popup = CustomerInGame.transform.Find("Popup");
        Main.TogglePopup(Popup.gameObject);
        IsServiced = false;
        yield return CustomerInGame.transform.DOMove(GameData<Main>.Boot.PointStartPeople.position, Main.AnimationMoveTime).SetEase(Ease.InCirc).WaitForCompletion();

        if (CustomerInGame != null && Customer != null)
        {
            GameData<Main>.Boot.DeleteCustomer(CustomerInGame);
            Customer = null;
        }

        PotionInfo.ClosePopup<PeopleImplementation>();

        GameData<Main>.Boot.GetComponent<MonoBehaviour>().StartCoroutine(Enter());
        yield break;
    }
}

public  class PeopleMaster
{
    private static List<BasePeople> customer = new();
    private static List<BasePeople> trader = new();

    private static List<BasePeople> pullCustomer = new();
    private static List<BasePeople> pullTrader = new();
    public static BasePeople GetRandCustomer() => GetNexst(pullCustomer, customer);
    public static BasePeople GetRandTrader() => GetNexst(pullTrader, trader);
    public static BasePeople GetNexst(List<BasePeople> peoples, List<BasePeople> parent)
    {
        if (peoples.Count == 0) {
            peoples.AddRange(parent);
            if (parent == customer) Debug.LogWarning("Пустой список customer peoples и были добавлены все");
        }
        
        BasePeople people = peoples[peoples.Count - 1];
        peoples.Remove(people);
        if (peoples.Count == 0) {
            peoples.AddRange(parent);
        }
        return people;
    }
    public static void GeneratePullOfDifity(Difity minDifity, Difity maxDifity, int count)
    {
        pullCustomer.Clear();
        AllPotion allPotion = CMS.Get<AllPotion>();

        List<int> IDs = allPotion.GetIDsOfDifity(minDifity);
        List<BasePeople> peoples = GetPeoplesOfIDPotions(IDs);
        for (int i = 0; i < count - 1; i++) {
            pullCustomer.Add(peoples[Random.Range(0, peoples.Count)]);
        }
        foreach (BasePeople people in pullCustomer) {
            SetPotion(people, IDs, allPotion);
        }

        if (count > 1) {
            IDs = allPotion.GetIDsOfDifity(maxDifity);
            peoples = GetPeoplesOfIDPotions(IDs);
            BasePeople people = peoples[Random.Range(0, peoples.Count)];
            SetPotion(people, IDs, allPotion);
            pullCustomer.Add(people);
        }
    }
    private static void SetPotion(BasePeople people, List<int> IDs, AllPotion allPotion)
    {
        int[] IDsIn = people.DataComponent.IDsPotions;
        IDsIn = IDsIn.Intersect(IDs).ToArray();
        people.SetPotion(allPotion.GetByID(IDsIn[Random.Range(0, IDsIn.Length)]));
    }
    public static void Load()
    {
        customer = new();
        trader = new();

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

        BasePeople[] peoples = {
            customer.FirstOrDefault(x => x is Anya),
            customer.FirstOrDefault(x => x is Foley),
            customer.FirstOrDefault(x => x is Backquit)
        };
        for (int i = 0; i < peoples.Length; i++)
        {
            peoples[i].SetPotionFirst();
        }
        pullCustomer.AddRange(peoples);
        pullTrader.AddRange(trader);
    }
    public static List<BasePeople> GetPeoplesOfIDPotions(List<int> IDs)
    {
        List<BasePeople> peoples = new();
        foreach (BasePeople people in customer) {
            foreach (int ID in IDs) {
                if (people.DataComponent.IDsPotions.Contains(ID)) {
                    peoples.Add(people);
                    break;
                }
            }
        }
        return peoples;
    }
}
