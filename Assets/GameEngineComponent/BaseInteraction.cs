using Engin.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

public abstract class BaseInteraction
{
    public Priority PriorityInteraction { get => Priority.Medium; set { } }
}

interface IEnterInStart
{
    void Start();
}

interface IEnterInReady
{
    void Start();
}

interface IEnterInUpdate
{
    void Update(float TimeDelta);
}

interface IEnterInPeople
{
    IEnumerator Enter();
    IEnumerator Exit();
}

interface IUpdatePotionInfo
{
    void UpdateInfo();
}

interface IUpdatePotionEvent
{
    void Event();
}

interface IUpdateGameData
{
    void LoadGameData();
    void UpdateMoney(int value, int delta);
    void UpdateReputation(float value, float delta);
}


interface IUpdateTutorialInfo
{
    void Update();
}

