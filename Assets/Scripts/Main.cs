using Engin.Utility;
using UnityEngine;

public class Main : MonoBehaviour, IMain
{
    public Interaction Interact = new Interaction();
    
    

    public void StartGame()
    {
        Interact.Init();
        CMS.Init();
        
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
    }
    
    
    public class Test : BaseInteraction, IEnterInStart
    {
        public void Start()
        {
          
        }
    }

    public void UpdateGame(float TimeDelta)
    { }

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