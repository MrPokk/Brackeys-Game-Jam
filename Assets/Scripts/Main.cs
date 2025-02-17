using Engin.Utility;
using UnityEngine;
using UnityEngine.UIElements;

public class Main : MonoBehaviour, IMain
{
    private Interaction Interact = new Interaction();
    public StoreIngredients Store;
    public Cauldron Cauldron;

    private Camera myCam;
    private ObjectIngredient InTheHand;

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


    public void NextStep()
    {
        var Ingredients = CMS.Get<DataIngredients>();
        foreach (var Element in Ingredients.prefabs) {
            GameData<Main>.Boot.Store.Add(Element);
        }    
        myCam = Camera.main;
    }


    public void UpdateGame(float TimeDelta)
    {
        var Update = Interact.FindAll<IEnterInUpdate>();
        foreach (var Element in Update)
        {
            Element.Update(TimeDelta);
        }

        if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse) && InTheHand == null) {
            RaycastHit2D hit = Physics2D.Raycast(myCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null) {
                IRaise raise = hit.collider.gameObject.GetComponent<IRaise>();
                if (raise != null) {
                    InTheHand = new(hit.collider.gameObject);
                    Store.Remove(InTheHand.Prefab);
                }
            }
        }
        else if (Input.GetMouseButtonUp((int)MouseButton.LeftMouse) && InTheHand != null) {
            if (InTheHand.Ingredient != null) {
                if (Cauldron.Near(myCam.ScreenToWorldPoint(Input.mousePosition))) {
                    Cauldron.Add(InTheHand);
                }
                else {
                    Store.Move(InTheHand.Prefab);
                }
            }
            InTheHand = null;
        }

        if (InTheHand != null) {
            InTheHand.Prefab.transform.position = (Vector2) myCam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetKeyDown(KeyCode.C)) {
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

class MyDebug : BaseInteraction, IEnterInUpdate
{
    void IEnterInUpdate.Update(float TimeDelta)
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
           var LicoriceRoot = CMS.Get<DataIngredients>().prefabs[0];
           GameData<Main>.Boot.Store.Add(LicoriceRoot);
        }
    }
}