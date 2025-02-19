using DG.Tweening;
using Engin.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
/*

 Код

 TODO: Придумать как хранить Параметры зелья для передачи его в BasePeople

 TODO: При наведёте на ингридиент появления эффектов (В отдельной панельки)

 Арт
 TODO: Перерисовать бэкграунд +-
 TODO: Добавить анимации
 TODO: Поменять шрифт
 TODO: Постпроцесинг

 TODO: Костер

Потом
TODO: Добавить менеджер всех текстов

 Баги
 TODO: Можно кинуть предмет за край экрана;

 */
public class Main : MonoBehaviour, IMain
{
    public TextManager TextManager;

    public Interaction Interact = new Interaction();
    public StoreIngredients Store;
    public ShopIngredients Shop;
    public Cauldron Cauldron;
    public PotionZone PotionZone;

    public GameObject PotionInfo;

    private Camera myCam;
    private Raise InTheHand;



    public Transform PointStartPeople;
    public Transform PointEndPeople;

    public float AnimationScale => 0.5f;
    public float AnimationScaleTime => 0.5f;
    public float AnimationMove => 0.5f;
    public float AnimationMoveTime => 0.3f;
    public void Awake()
    {
        CMS.Init();
        GameData<Main>.Boot = this;
    }
    public void StartGame()
    {

        Interact.Init();
        var Ready = Interact.FindAll<IEnterInReady>();
        var Start = Interact.FindAll<IEnterInStart>();
        foreach (var Element in Start)
        {
            Element.Start();
        }

        foreach (var Element in Ready)
        {
            Element.Start();
        }


        GameData<Main>.IsStartGame = true;
        NextStep();
    }


    private void NextStep()
    {
        foreach (var Element in CMS.Get<AllIngredients>().Ingredients)
        {
            GameData<Main>.Boot.Store.Add(Element);
        }

        var PeopleUpdate = Interact.FindAll<IEnterInPeople>();
        foreach (var Element in PeopleUpdate)
        {
            StartCoroutine(Element.Enter());
        }


        myCam = Camera.main;
    }

    public GameObject AddCustomer(BasePeople Customer)
    {
        return Instantiate(Customer.DataComponent.Prefab, GameData<Main>.Boot.PointStartPeople.position, new Quaternion());
    }
    public void DeleteCustomer(GameObject Customer)
    {
        Destroy(Customer);
    }

    public void UpdateGame(float TimeDelta)
    {
        var Update = Interact.FindAll<IEnterInUpdate>();


        foreach (var Element in Update)
        {
            Element.Update(TimeDelta);
        }



        if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse) && InTheHand == null)
            LeftClick();
        else if (Input.GetMouseButtonUp((int)MouseButton.LeftMouse) && InTheHand != null)
            RightClick();

        if (InTheHand != null)
        {
            InTheHand.transform.DOMove(new(myCam.ScreenToWorldPoint(Input.mousePosition).x, myCam.ScreenToWorldPoint(Input.mousePosition).y, 0), GameData<Main>.Boot.AnimationMoveTime).SetEase(Ease.Flash);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Cauldron.Cook();
        }
    }

    private void LeftClick()
    {
        RaycastHit2D hit = Physics2D.Raycast(myCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            Raise raise = hit.collider.gameObject.GetComponent<Raise>();

            if (raise != null)
            {
                if (raise is Ingredient ingredient)
                {
                    if (Store.Contains(ingredient))
                        Store.Remove(ingredient);
                    else if (Shop.Contains(ingredient))
                    {
                        if (ingredient.Price > GameData<Main>.Money) return;
                        Shop.Remove(ingredient);
                        GameData<Main>.Money -= ingredient.Price;
                    }
                }
                else if (raise.gameObject == PotionZone.PotionIn)
                {
                    PotionZone.Remove();
                }
                InTheHand = raise;
            }
            else
            {
                if (hit.collider.gameObject.GetComponent<CustomButton>() is CustomButton Button)
                {
                    Button.Click();
                }
            }
        }
    }
    private void RightClick()
    {
        if (InTheHand is Ingredient ingredient)
        {
            if (Cauldron.Near(myCam.ScreenToWorldPoint(Input.mousePosition)))
            {
                Cauldron.Add(ingredient);
            }
            else
            {
                Store.Move(ingredient);
            }
        }
        else if (InTheHand is Potion potion && PotionZone.Near(myCam.ScreenToWorldPoint(Input.mousePosition)))
        {
            PotionZone.Add(potion.gameObject);
        }

        InTheHand = null;
    }

    public void PhysicUpdateGame(float TimeDelta)
    { }

    public void Update()
    {
        UpdateGame(Time.deltaTime);
    }

    public void FixedUpdate()
    {
        PhysicUpdateGame(Time.deltaTime);
    }

    public void Start()
    {
        StartGame();
    }

    public static void TogglePopup(GameObject Popup)
    {
        Popup.SetActive(!Popup.activeSelf);
    }


}

class GameDataInfo : BaseInteraction
{
    
}

class PotionInfo : BaseInteraction, IUpdatePotionInfo
{
    private TMP_Text Name = GameData<Main>.Boot.TextManager.Get("PotionInfoName");

    private TMP_Text Description = GameData<Main>.Boot.TextManager.Get("PotionInfoDescription");
    private List<EffectData> Effects => GameData<Main>.Boot.Cauldron.effectsMaster.Get();
    public void UpdateInfo()
    {
        var StringsElementData = new List<string>();


        var Potion = GameData<Main>.Boot.Cauldron.PreCook();
        Name.SetText(Potion.name);

        foreach (var Element in Effects)
        {
            StringsElementData.Add($"{Element.Type.ToString().ToUpperInvariant()}: {Element.Power}");
        }
        Description.SetText("");
        Description.SetText(string.Join("\n", StringsElementData));
    }
}



class PotionEvent : BaseInteraction, IUpdatePotionEvent
{
    public void Event()
    { }
}

class MyDebug : BaseInteraction, IEnterInUpdate
{
    void IEnterInUpdate.Update(float TimeDelta)
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            var LicoriceRoot = CMS.Get<AllIngredients>().Ingredients[Random.Range(0, CMS.Get<AllIngredients>().Ingredients.Count)];
            GameData<Main>.Boot.Store.Add(LicoriceRoot);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            var All = CMS.Get<AllIngredients>().Ingredients;
            foreach (var Element in All)
                GameData<Main>.Boot.Store.Add(Element);
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            GameData<Main>.Boot.Shop.Generatre(Random.Range(3, 7));
        }
    }

}

public class PeopleImplementation : BaseInteraction, IEnterInPeople
{

    public GameObject CustomerInGame { get; private set; }
    public static BasePeople Customer { get; private set; } = null;
    public bool IsServiced { get; private set; } = false;

    public IEnumerator Enter()
    {
        if (Customer == null && !IsServiced)
        {
            var AllVarPeoples = CMS.GetAll<BasePeople>();
            Customer = AllVarPeoples[Random.Range(0, AllVarPeoples.Count)];

            yield return new WaitForSeconds(5f);
            CustomerInGame = GameData<Main>.Boot.AddCustomer(Customer);
            var Popup = CustomerInGame.transform.Find("Popup").gameObject;
            Main.TogglePopup(Popup);

            yield return CustomerInGame.transform.DOMove(GameData<Main>.Boot.PointEndPeople.position, GameData<Main>.Boot.AnimationMoveTime + 1f).SetEase(Ease.OutCirc).WaitForCompletion();

            yield return new WaitForSeconds(.5f);
            // Запуск звука 
            Main.TogglePopup(Popup);

            //  var AllTextComponent = CustomerInGame.GetComponentsInChildren<TMP_Text>();
            // var Description = AllTextComponent.First(Text => (Text.name == "Description"));

            CustomerInGame.transform.DOComplete();

            IsServiced = true;

        }
        yield break;
    }
    public IEnumerator Exit()
    {
        var Popup = CustomerInGame.transform.Find("Popup").gameObject;
        Main.TogglePopup(Popup);

        yield return new WaitForSeconds(1f);

        yield return CustomerInGame.transform.DOMove(GameData<Main>.Boot.PointStartPeople.position, GameData<Main>.Boot.AnimationMoveTime + .5f).SetEase(Ease.InCirc).WaitForCompletion();

        CustomerInGame.transform.DOComplete();
        
        GameData<Main>.Boot.DeleteCustomer(CustomerInGame);
        Customer = null;
        IsServiced = false;

        GameData<Main>.Boot.GetComponent<MonoBehaviour>().StartCoroutine(Enter());
        yield break;
    }

}
