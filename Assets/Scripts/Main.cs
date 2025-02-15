using Engin.Utility;
using UnityEngine;

public class Main : MonoBehaviour, IMain
{
    public Interaction Interact { get; set; }

    public void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        Interact.Init();

        Debug.Log("StartGame");
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

    public void UpdateGame(float TimeDelta)
    {
    }

    public void PhysicUpdateGame(float TimeDelta)
    {
    }
}