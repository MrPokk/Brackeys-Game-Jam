using DG.Tweening;
using Engin.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using MouseButton = UnityEngine.UIElements.MouseButton;
using Random = UnityEngine.Random;
/*

 Код

 на завтра

 Арт
 TODO: Перерисовать бэкграунд +-
 TODO: Добавить анимации +-
 TODO: Поменять шрифт +
 TODO: Постпроцесинг -





 TODO: Костер -


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

    public GameObject PotionInfoCauldron;
    public GameObject PotionInfoCustomer;
    public GameObject ToolKit;

    private Camera myCam;
    private Raise InTheHand;

    public Transform PointStartPeople;
    public Transform PointEndPeople;

    public const float AnimationScale = 0.5f;
    public const float AnimationScaleTime = 0.5f;
    public const float AnimationMove = 0.5f;
    public const float AnimationMoveTime = 0.3f;


    public const float ReputationDebuff = 20f;

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

        var GamaData = Interact.FindAll<GameDataInfo>();
        foreach (var Element in GamaData)
        {
            Element.Update();
        }

        Interact.FindAll<PeopleImplementation>();
        Interact.FindAll<PotionZone>();
        Interact.FindAll<MyDebug>();
        Interact.FindAll<IUpdatePotionInfo>();

        PotionInfoCauldron.SetActive(false);
        PotionInfoCustomer.SetActive(false);
        ToolKit.SetActive(false);

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


        foreach (var Element in InteractionCache<IUpdatePotionInfo>.AllInteraction)
        {
            Element.UpdateInfo();
        }

        foreach (var Element in Update)
        {
            Element.Update(TimeDelta);
        }

        if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse) && InTheHand == null)
            LeftClick();
        else if (Input.GetMouseButtonUp((int)MouseButton.LeftMouse) && InTheHand != null)
            RightClick();

        HoverMouse();

        if (InTheHand != null)
        {
            InTheHand.transform.DOMove(new(myCam.ScreenToWorldPoint(Input.mousePosition).x, myCam.ScreenToWorldPoint(Input.mousePosition).y, 0), Main.AnimationMoveTime).SetEase(Ease.Flash);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Cauldron.Cook();
        }
    }

    private void HoverMouse()
    {
        RaycastHit2D hit = Physics2D.Raycast(myCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null && InTheHand == null)
        {
            ToolKit.transform.position = hit.point + new Vector2(-1.8f, 1.5f);
            Raise raise = hit.collider.gameObject.GetComponent<Raise>();

            if (raise is Ingredient ingredient)
            {
                var EffectsInIngredient = new List<string>();
                TMP_Text Name = GameData<Main>.Boot.TextManager.Get("ToolKitNameObject");

                TMP_Text Effect = GameData<Main>.Boot.TextManager.Get("ToolKitEffectObject");
                TMP_Text Description = GameData<Main>.Boot.TextManager.Get("ToolKitDescriptionObject");

                List<EffectRange> EffectsInCustomer = PeopleImplementation.Customer.DataComponent.TypePoison.Recipe;

                foreach (var Element in ingredient.Effects)
                {
                    if (PeopleImplementation.Customer != null && PeopleImplementation.Customer.DataComponent.Type != TypePeople.Trader)
                    {
                        var EffectCustomer = EffectsInCustomer.FirstOrDefault(range => {
                            if (range.Type == Element.Type)
                                return true;
                            return false;
                        });
                        if (EffectCustomer != null && EffectCustomer.Type == Element.Type)
                        {
                            var Color = (CMS.Get<AllEffect>().GetAtID(Element.Type).Color);
                            Color.a = 1f;
                            var ColorHex = $"#{XColor.ToHexString(Color)}";
                            EffectsInIngredient.Add($"<color={ColorHex}>{Element.Type.ToString().ToUpperInvariant()}</color>: {Element.Power}");
                        }
                        else
                        {
                            var ColorBad = $"#373737";
                            EffectsInIngredient.Add($"<color={ColorBad}>{Element.Type.ToString().ToUpperInvariant()}</color>: {Element.Power}");
                        }
                    }
                    else
                    {
                        EffectsInIngredient.Add($"{Element.Type.ToString().ToUpperInvariant()}: {Element.Power}");
                    }
                }

                Name.SetText(ingredient.Name);
                Description.SetText(ingredient.Description);
                Effect.SetText(string.Join("\n", EffectsInIngredient));

                GameData<Main>.Boot.ToolKit.SetActive(true);
            }
            else if (raise is Potion potion)
            {

                TMP_Text Name = GameData<Main>.Boot.TextManager.Get("ToolKitNameObject");

                TMP_Text Effect = GameData<Main>.Boot.TextManager.Get("ToolKitEffectObject");
                TMP_Text Description = GameData<Main>.Boot.TextManager.Get("ToolKitDescriptionObject");

                var EffectsInPotionName = new List<string>();


                List<EffectData> EffectsInPotion = potion.effects;


                foreach (var Element in EffectsInPotion)
                {
                    EffectsInPotionName.Add($"{CMS.Get<AllEffect>().GetAtID(Element.Type).Name}: {Element.Power}");
                }
                Name.SetText(potion.Name);
                Description.SetText(potion.Descriptions);
                Effect.SetText(string.Join("\n", EffectsInPotionName));

                GameData<Main>.Boot.ToolKit.SetActive(true);
            }
        }
        else
        {
            GameData<Main>.Boot.ToolKit.SetActive(false);
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
        if (Popup != null)
            Popup.SetActive(!Popup.activeSelf);
    }
}

class GameDataInfo : BaseInteraction, IUpdateGameData
{
    public void Update()
    {
        GameData<Main>.Boot.TextManager.Get("Money").SetText(GameData<Main>.Money.ToString());
        GameData<Main>.Boot.TextManager.Get("Reputation").SetText(GameData<Main>.Reputation.ToString());
    }
    public static void LoseGame()
    {
        
    }
    public static void WinGame()
    {
        
    }
}
class PotionInfo : BaseInteraction, IUpdatePotionInfo
{
    private TMP_Text Name = GameData<Main>.Boot.TextManager.Get("PotionInfoName");
    private TMP_Text NameCustomerPotion = GameData<Main>.Boot.TextManager.Get("PotionInfoCustomerPotionName");


    private TMP_Text Description = GameData<Main>.Boot.TextManager.Get("PotionInfoDescription");

    private TMP_Text NeedCraftText = GameData<Main>.Boot.TextManager.Get("PotionInfoNeed");

    private static Vector3 SIZE_INFO_CAULDRON = GameData<Main>.Boot.PotionInfoCauldron.transform.localScale;
    private static Vector3 SIZE_INFO_CUSTOMER = GameData<Main>.Boot.PotionInfoCustomer.transform.localScale;
    public static void OpenPopup<T>()
    {
        Type WhoCalled = typeof(T);
        if (WhoCalled == typeof(Cauldron))
        {
            GameData<Main>.Boot.PotionInfoCauldron.SetActive(true);
            GameData<Main>.Boot.PotionInfoCauldron.transform.DOScale(SIZE_INFO_CAULDRON, Main.AnimationScaleTime);
        }
        else if (WhoCalled == typeof(PeopleImplementation) && PeopleImplementation.Customer.DataComponent.Type != TypePeople.Trader)
        {
            GameData<Main>.Boot.PotionInfoCustomer.SetActive(true);
            GameData<Main>.Boot.PotionInfoCustomer.transform.DOScale(SIZE_INFO_CUSTOMER, Main.AnimationScaleTime);
        }
    }
    public static void ClosePopup<T>()
    {
        Type WhoCalled = typeof(T);
        if (WhoCalled == typeof(Cauldron))
        {
            GameData<Main>.Boot.PotionInfoCauldron
                .transform.DOScale(0, Main.AnimationScaleTime).OnComplete(
                    (() => Main.TogglePopup(GameData<Main>.Boot.PotionInfoCauldron)));
        }
        else if (WhoCalled == typeof(PeopleImplementation))
        {
            GameData<Main>.Boot.PotionInfoCustomer
                .transform.DOScale(0, Main.AnimationScaleTime).OnComplete(
                    (() => Main.TogglePopup(GameData<Main>.Boot.PotionInfoCustomer)));
        }
    }


    public void UpdateInfo()
    {
        var EffectsInCauldron = GameData<Main>.Boot.Cauldron.effectsMaster.Get();

        List<EffectRange> EffectsInCustomer = new List<EffectRange>();
        if (PeopleImplementation.Customer != null && PeopleImplementation.Customer.DataComponent.TypePoison.Recipe.Any())
            EffectsInCustomer.AddRange(PeopleImplementation.Customer.DataComponent.TypePoison.Recipe);
        else return;

        var EffectsInCauldronText = new List<string>();
        var EffectsInCraftText = new List<string>();

        var Potion = GameData<Main>.Boot.Cauldron.GetEffectPotion();
        Name.SetText(Potion.Name);


        foreach (var ElementCauldron in EffectsInCauldron)
        {

            if (PeopleImplementation.Customer != null && PeopleImplementation.Customer.DataComponent.Type != TypePeople.Trader)
            {

                var EffectCustomer = EffectsInCustomer.FirstOrDefault(range => {
                    if (range.Type == ElementCauldron.Type)
                        return true;
                    return false;
                });
                if (EffectCustomer != null && EffectCustomer.Type == ElementCauldron.Type)
                {
                    var Color = (CMS.Get<AllEffect>().GetAtID(ElementCauldron.Type).Color);
                    Color.a = 1f;
                    var ColorHex = $"#{XColor.ToHexString(Color)}";
                    EffectsInCauldronText.Add($"<color={ColorHex}>{ElementCauldron.Type.ToString().ToUpperInvariant()}</color>: {ElementCauldron.Power}");
                }
                else
                {
                    var ColorBad = $"#373737";
                    EffectsInCauldronText.Add($"<color={ColorBad}>{ElementCauldron.Type.ToString().ToUpperInvariant()}</color>: {ElementCauldron.Power}");
                }
            }
            else
            {
                EffectsInCauldronText.Add($"{ElementCauldron.Type.ToString().ToUpperInvariant()}: {ElementCauldron.Power}");
            }

        }

        if (PeopleImplementation.Customer != null)
        {
            foreach (var Element in EffectsInCustomer)
            {
                var Color = (CMS.Get<AllEffect>().GetAtID(Element.Type).Color);
                Color.a = 1f;
                var ColorHex = $"#{XColor.ToHexString(Color)}";
                EffectsInCraftText.Add($"<color={ColorHex}>{Element.Type.ToString().ToUpperInvariant()}</color>: {Element.Min} to {Element.Max}");
            }
        }

        NameCustomerPotion.SetText(PeopleImplementation.Customer.DataComponent.TypePoison.Name + " " + PeopleImplementation.Customer.DataComponent.TypePoison.ID);
        Description.SetText(string.Join("\n", EffectsInCauldronText));
        NeedCraftText.SetText(string.Join("\n", EffectsInCraftText));
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
            var AllVarPeoples = CMS.GetAll<BasePeople>();
            Customer = AllVarPeoples[Random.Range(0, AllVarPeoples.Count)].ModifyDataSet();

            yield return new WaitForSeconds(1f);
            CustomerInGame = GameData<Main>.Boot.AddCustomer(Customer);
            var Popup = CustomerInGame.transform.Find("Popup").gameObject;
            Main.TogglePopup(Popup);

            yield return CustomerInGame.transform.DOMove(GameData<Main>.Boot.PointEndPeople.position, Main.AnimationMoveTime + 1f).SetEase(Ease.OutCirc).WaitForCompletion();

            yield return new WaitForSeconds(.5f);
            // Запуск звука 
            Main.TogglePopup(Popup);

            if (CustomerInGame != null)
                CustomerInGame.transform.DOComplete();

            PotionInfo.OpenPopup<PeopleImplementation>();

            IsServiced = true;

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
