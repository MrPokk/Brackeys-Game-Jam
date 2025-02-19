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
 TODO: Сделать систему Зелий +/-
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
    private Interaction Interact = new Interaction();
    public StoreIngredients Store;
    public Cauldron Cauldron;
    public PotionZone PotionZone;

    private Camera myCam;
    private ObjectIngredient InTheHand;

    public Transform PointStartPeople;
    public Transform PointEndPeople;

    public float AnimationScale => 0.5f;
    public float AnimationScaleTime => 0.5f;
    public float AnimationMove => 0.5f;
    public float AnimationMoveTime => 0.3f;

    public void StartGame()
    {
        Interact.Init();
        CMS.Init();
        GameData<Main>.Boot = this;

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
        foreach (var Element in CMS.Get<AllIngredients>().Prefabs)
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

    public void UpdateGame(float TimeDelta)
    {
        var Update = Interact.FindAll<IEnterInUpdate>();
        foreach (var Element in Update)
        {
            Element.Update(TimeDelta);
        }

        if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse) && InTheHand == null)
        {
            RaycastHit2D hit = Physics2D.Raycast(myCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                IRaise raise = hit.collider.gameObject.GetComponent<IRaise>();
                if (raise != null)
                {
                    InTheHand = new(hit.collider.gameObject);
                    Store.Remove(InTheHand.Prefab);
                    if (InTheHand.Prefab == PotionZone.PotionIn)
                    {
                        PotionZone.Remove();
                    }
                }
            }
        }
        else if (Input.GetMouseButtonUp((int)MouseButton.LeftMouse) && InTheHand != null)
        {

            if (InTheHand.Ingredient != null)
            {
                if (Cauldron.Near(myCam.ScreenToWorldPoint(Input.mousePosition)))
                {
                    Cauldron.Add(InTheHand);
                }
                else
                {
                    Store.Move(InTheHand.Prefab);
                }
            }
            else if (InTheHand.Prefab.GetComponent<Potion>() != null && PotionZone.Near(myCam.ScreenToWorldPoint(Input.mousePosition)))
            {
                PotionZone.Add(InTheHand);
            }

            InTheHand = null;
        }

        if (InTheHand != null)
        {
            InTheHand.Prefab.transform.DOMove(new(myCam.ScreenToWorldPoint(Input.mousePosition).x, myCam.ScreenToWorldPoint(Input.mousePosition).y, 0), GameData<Main>.Boot.AnimationMoveTime).SetEase(Ease.Flash);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Cauldron.Cook();
        }
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
}

class PotionInfo : MonoBehaviour
{
   [SerializeField]private TMP_Text Name;
   [SerializeField]private TMP_Text Description;
   private List<EffectData> Effects => GameData<Main>.Boot.Cauldron.effectsMaster.Get();


   private void UpdateInfo()
   {
      // Description.text += $"\n{}";
   }
}

class MyDebug : BaseInteraction, IEnterInUpdate
{
    void IEnterInUpdate.Update(float TimeDelta)
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            var LicoriceRoot = CMS.Get<AllIngredients>().Prefabs[Random.Range(0, CMS.Get<AllIngredients>().Prefabs.Count)];
            GameData<Main>.Boot.Store.Add(LicoriceRoot);
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
            TogglePopup(Popup);

            yield return CustomerInGame.transform.DOMove(GameData<Main>.Boot.PointEndPeople.position, GameData<Main>.Boot.AnimationMoveTime + 1f).SetEase(Ease.OutCirc).WaitForCompletion();

            yield return new WaitForSeconds(.5f);
            // Запуск звука 
            TogglePopup(Popup);

            var AllTextComponent = CustomerInGame.GetComponentsInChildren<TMP_Text>();
            var Description = AllTextComponent.First(Text => (Text.name == "Description"));

            IsServiced = true;
        }

        yield break;

    }

    private void TogglePopup(GameObject Popup)
    {
        Popup.SetActive(!Popup.activeSelf);
    }

}
