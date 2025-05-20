using DG.Tweening;
using Engin.Utility;
using SmallHedge.SoundManager;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using MouseButton = UnityEngine.UIElements.MouseButton;
/*

 Арт
 TODO: Добавить анимации +-

 TODO: Костер -


 */
public class Main : MonoBehaviour, IMain
{
    
    public LoadScene LoadScene;
    public TextManager TextManager;
    public TutorialManager TutorialManager;

    public Interaction Interact = new Interaction();
    public StoreIngredients Store;
    public ShopIngredients Shop;
    public Cauldron Cauldron;
    public PotionZone PotionZone;


    public GameObject PotionInfoCauldron;
    public GameObject PotionInfoCustomer;
    public GameObject WinPopup;
    public GameObject LosePopup;
    public GameObject ToolKit;

    public Camera myCam { get; private set; }
    private Raise InTheHand;

    public Transform PointStartPeople;
    public Transform PointEndPeople;

    public const float AnimationScale = 0.5f;
    public const float AnimationScaleTime = 0.5f;
    public const float AnimationMove = 0.5f;
    public const float AnimationMoveTime = 0.3f;


    public const float ReputationDebuff = 5f;

[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void CmsInit()
    {
        CMS.Init(); 
    }

    public void Awake()
    {
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
        myCam = Camera.main;
        PeopleMaster.Load();
        LoadScene.gameObject.SetActive(true);
        GameData<Main>.IsStartGame = true;
        NextStep();
    }
// Позже
    public void RestartGame()
    { }


    private void NextStep()
    {
        //foreach (var Element in CMS.Get<AllIngredients>().GetStartPull())
        foreach (var Element in CMS.Get<AllIngredients>().Ingredients)
            Store.Add(Element);

        var PeopleUpdate = Interact.FindAll<PeopleImplementation>();
        foreach (var Element in PeopleUpdate)
        {
            StartCoroutine(Element.Enter());
        }

        var PotionInfo = Interact.FindAll<PotionInfo>();
        foreach (var Element in PotionInfo) {
            Element.UpdateInfo();
        }

        var GamaData = Interact.FindAll<GameDataInfo>();
        foreach (var Element in GamaData)
        {
            Element.LoadGameData();
        }

        Interact.FindAll<PeopleImplementation>();
        Interact.FindAll<PotionZone>();
#if UNITY_EDITOR
        Interact.FindAll<MyDebug>();
#endif

        var TutorialInfo = Interact.FindAll<TutorialInfo>();
        foreach (var Element in TutorialInfo)
        {
            Element.Update();
        }

        TutorialManager.gameObject.SetActive(true);

        PotionInfoCauldron.SetActive(false);
        PotionInfoCustomer.SetActive(false);
        ToolKit.SetActive(false);
        LosePopup.SetActive(false);
        WinPopup.SetActive(false);

        var PlusMoney = GameData<Main>.Boot.TextManager.Get("Money Plus");
        var PlusReputation = GameData<Main>.Boot.TextManager.Get("Reputation Plus");

        PlusMoney.gameObject.SetActive(false);
        PlusReputation.gameObject.SetActive(false);

        StartCoroutine(LoadScene.Load());

        GameData<Main>.Reputation = 0;
        GameData<Main>.Money = 20;
        GameData<Main>.Win = false;
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
            LeftDown();
        else if (Input.GetMouseButtonUp((int)MouseButton.LeftMouse) && InTheHand != null)
            LeftUp();
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

    private static GameObject ObjectHit;
    private int OldLayerHit;
    private void OffToolKit()
    {
        if (ObjectHit != null && ObjectHit.GetComponent<Ingredient>() != null)
        {
            ObjectHit.GetComponent<SpriteRenderer>().sortingOrder = OldLayerHit;
        }
        ObjectHit = null;
        ToolKit.SetActive(false);
    }
    private void HoverMouse()
    {
        if (PauseMenu.Paused) return;

        RaycastHit2D hit = Physics2D.Raycast(myCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null && InTheHand == null)
        {
            if (ToolKit.activeSelf && hit.collider.gameObject == ObjectHit)
            {
                return;
            }

            if (ObjectHit != null)
            {
                ObjectHit.GetComponent<SpriteRenderer>().sortingOrder = OldLayerHit;
            }
            GameObject OldObjectHit = ObjectHit;
            ObjectHit = hit.collider.gameObject;
            ToolKit.transform.position = ObjectHit.transform.position;

            Raise raise = ObjectHit.GetComponent<Raise>();
            if (raise == null)
            {
                ButtonSack button = ObjectHit.GetComponent<ButtonSack>();
                if (button != null)
                {
                    TextManager.Get("ToolKitNameObject").SetText(button.Name);
                    TextManager.Get("ToolKitEffectObject").SetText(button.Description);
                    TextManager.Get("ToolKitDescriptionObject").SetText($"Цена {button.price}");
                    ToolKit.SetActive(true);
                    return;
                }
                OffToolKit();
                return;
            }
            if (raise is Ingredient ingredient)
            {
                SpriteRenderer sp = ObjectHit.GetComponent<SpriteRenderer>();
                OldLayerHit = sp.sortingOrder;
                ObjectHit.GetComponent<SpriteRenderer>().sortingOrder = OldLayerHit + 10;

                var EffectsInIngredient = new List<string>();
                TMP_Text Name = TextManager.Get("ToolKitNameObject");

                TMP_Text Effect = TextManager.Get("ToolKitEffectObject");
                TMP_Text Description = TextManager.Get("ToolKitDescriptionObject");

                Name.SetText(ingredient.Name);
                if (raise is Catalyst)
                {
                    Description.SetText(string.Empty);
                    Effect.SetText(ingredient.Description);
                }
                else
                {
                    var EffectsInCustomer = PeopleImplementation.Customer.DataComponent.TypePoison.Recipe;

                    foreach (var Element in ingredient.Effects)
                    {
                        var SelectEffect = CMS.Get<AllEffect>().GetAtID(Element.Type);
                        if (PeopleImplementation.Customer != null && PeopleImplementation.Customer.DataComponent.Type != TypePeople.Trader)
                        {
                            var EffectCustomer = EffectsInCustomer.FirstOrDefault(x => x.Type == Element.Type);
                            if (EffectCustomer != null && EffectCustomer.Type == Element.Type)
                            {
                                var Color = SelectEffect.Color;
                                Color.a = 1f;
                                var ColorHex = $"#{XColor.ToHexString(Color)}";
                                EffectsInIngredient.Add($"<color={ColorHex}>{SelectEffect.Name.ToUpperInvariant()}:</color> {Element.Power}");
                            }
                            else
                            {
                                var ColorBad = $"#373737";
                                EffectsInIngredient.Add($"<color={ColorBad}>{SelectEffect.Name.ToUpperInvariant()}:</color> {Element.Power}");
                            }
                        }
                        else
                        {
                            EffectsInIngredient.Add($"{SelectEffect.Name.ToUpperInvariant()}: {Element.Power}");
                        }
                    }

                    Description.SetText(ingredient.Description);
                    Effect.SetText(string.Join("\n", EffectsInIngredient));
                }
                ToolKit.SetActive(true);
            }
            else if (raise is Potion potion)
            {

                TMP_Text Name = TextManager.Get("ToolKitNameObject");

                TMP_Text Effect = TextManager.Get("ToolKitEffectObject");
                TMP_Text Description = TextManager.Get("ToolKitDescriptionObject");

                var EffectsInPotionName = new List<string>();


                List<EffectData> EffectsInPotion = potion.effects;


                foreach (var Element in EffectsInPotion)
                {
                    EffectsInPotionName.Add($"{CMS.Get<AllEffect>().GetAtID(Element.Type).Name}: {Element.Power}");
                }

                if (PeopleImplementation.Customer.DataComponent.TypePoison.ID == potion.ID)
                {

                    TMP_Text NameCauldronPotion = GameData<Main>.Boot.TextManager.Get("PotionInfoName");
                    TMP_Text NameCustomerPotion = GameData<Main>.Boot.TextManager.Get("PotionInfoCustomerPotionName");
                    NameCauldronPotion.SetText($"<color=#22f814>{potion.Name}</color>");
                    NameCustomerPotion.SetText($"<color=#22f814>{PeopleImplementation.Customer.DataComponent.TypePoison.Name}</color>");

                    Name.SetText($"<color=#22f814>{potion.Name}</color>");
                    //  NameCauldronPotion.gameObject.transform.DOShakeScale(1f, Main.AnimationScaleTime,0,0).OnComplete(() => {
                    //    NameCustomerPotion.gameObject.transform.DOShakeScale(1f, Main.AnimationScaleTime,0,0).SetLoops( -1, LoopType.Yoyo );
                    // });

                }
                else
                {
                    TMP_Text NameCauldronPotion = GameData<Main>.Boot.TextManager.Get("PotionInfoName");
                    TMP_Text NameCustomerPotion = GameData<Main>.Boot.TextManager.Get("PotionInfoCustomerPotionName");
                    NameCauldronPotion.SetText($"<color=#de1111>{potion.Name}</color>");
                    NameCustomerPotion.SetText($"<color=#de1111>{PeopleImplementation.Customer.DataComponent.TypePoison.Name}</color>");

                    Name.SetText($"<color=#de1111>{potion.Name}</color>");
                    //   NameCauldronPotion.gameObject.transform.DOShakeScale(1f, Main.AnimationScaleTime,0,0).OnComplete(() => {
                    //     NameCustomerPotion.gameObject.transform.DOShakeScale(1f, Main.AnimationScaleTime,0,0).SetLoops( -1, LoopType.Yoyo );
                    //});

                }

                Description.SetText(potion.Descriptions);
                Effect.SetText(string.Join("\n", EffectsInPotionName));

                GameData<Main>.Boot.ToolKit.SetActive(true);
            }
        }
        else
        {
            OffToolKit();
        }
    }
    private void LeftDown()
    {
        RaycastHit2D hit = Physics2D.Raycast(myCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            Raise raise = hit.collider.gameObject.GetComponent<Raise>();

            if (raise != null)
            {
                SoundManager.PlaySound(SoundType.OpenPopup);
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

    private void LeftUp()
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

        Vector3 pos = myCam.ScreenToWorldPoint(Input.mousePosition);
        pos.z = InTheHand.transform.position.z;
        if (!GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Camera.main), new Bounds(pos, Vector3.zero)))
        {
            Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)) * 0.9f;
            Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1)) * 0.9f;
            pos.x = Mathf.Clamp(pos.x, min.x, max.x);
            pos.y = Mathf.Clamp(pos.y, min.y, max.y);
            InTheHand.transform.DOMove(pos, AnimationMoveTime).SetEase(Ease.InOutElastic);
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
