using Engin.Utility;
using UnityEngine;

public class Main : MonoBehaviour, IMain
{
    private Interaction Interact = new Interaction();
    public StoreIngredients Store;
    
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
        var LicoriceRoot =  CMS.Get<LicoriceRoot>();
        GameData<Main>.Boot.Store.Add(LicoriceRoot);
    }


    public void UpdateGame(float TimeDelta)
    {
        var Update = Interact.FindAll<IEnterInUpdate>();
        foreach (var Element in Update)
        {
            Element.Update(TimeDelta);
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


class Debug : BaseInteraction, IEnterInUpdate
{
    void IEnterInUpdate.Update(float TimeDelta)
    {
        if (Input.GetKeyDown(KeyCode.D))
        {

           var LicoriceRoot =  CMS.Get<LicoriceRoot>();
           GameData<Main>.Boot.Store.Add(LicoriceRoot);
            
           
        }
    }
}