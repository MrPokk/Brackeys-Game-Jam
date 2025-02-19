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

 Баги
 TODO: Можно кинуть предмет за край экрана;

 */
public class Main : MonoBehaviour, IMain
{
    public int Money;
    public Interaction Interact = new Interaction();
    public StoreIngredients Store;
    public StoreIngredients Shop;
    public Cauldron Cauldron;
    public PotionZone PotionZone;

    public GameObject PotionInfo;
    public TMP_Text PotionName;
    public TMP_Text PotionDescription;

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
            GameData<Main>.Boot.Shop.Add(Element);
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

    public void LeftClick()
    {
        RaycastHit2D hit = Physics2D.Raycast(myCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null) {
            Raise raise = hit.collider.gameObject.GetComponent<Raise>();
            
            if (raise != null) {
                if (raise is Ingredient ingredient) {
                    if (Store.Contains(ingredient))
                        Store.Remove(ingredient);
                    else if (Shop.Contains(ingredient)) {
                        if (ingredient.Price > Money) return;
                        Shop.Remove(ingredient);
                        Money -= ingredient.Price;
                    }
                }
                else if (raise.gameObject == PotionZone.PotionIn) {
                    PotionZone.Remove();
                }
                InTheHand = raise;
            }

            if (hit.collider.gameObject.GetComponent<CustomButton>() is CustomButton Button)
            {
                Button.EventClose();
            }
        }
    }
    public void RightClick()
    {
        if (InTheHand is Ingredient ingredient) {
            if (Cauldron.Near(myCam.ScreenToWorldPoint(Input.mousePosition))) {
                Cauldron.Add(ingredient);
            }
            else {
                Store.Move(ingredient);
            }
        }
        else if (InTheHand is Potion potion && PotionZone.Near(myCam.ScreenToWorldPoint(Input.mousePosition))) {
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

class PotionInfo : BaseInteraction, IUpdatePotionInfo
{
    private TMP_Text Name = GameData<Main>.Boot.PotionName;
    private TMP_Text Description = GameData<Main>.Boot.PotionDescription;
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
    }
}

class PeopleImplementation : BaseInteraction, IEnterInPeople
{
    private BasePeople Customer = null;
    private bool IsServiced = false;

    public IEnumerator Enter()
    {
        if (Customer == null && !IsServiced)
        {
            var AllVarPeoples = CMS.GetAll<BasePeople>();
            var Customer = AllVarPeoples[Random.Range(0, AllVarPeoples.Count)];

            yield return new WaitForSeconds(5f);
            var CustomerInGame = GameData<Main>.Boot.AddCustomer(Customer);
            var Popup = CustomerInGame.transform.Find("Popup").gameObject;
            Main.TogglePopup(Popup);

            yield return CustomerInGame.transform.DOMove(GameData<Main>.Boot.PointEndPeople.position, GameData<Main>.Boot.AnimationMoveTime + 1f).SetEase(Ease.OutCirc).WaitForCompletion();

            yield return new WaitForSeconds(.5f);
            // Запуск звука 
            Main.TogglePopup(Popup);

            var AllTextComponent = CustomerInGame.GetComponentsInChildren<TMP_Text>();
            var Description = AllTextComponent.First(Text => (Text.name == "Description"));

            IsServiced = true;
        }

        yield break;

    }

}
