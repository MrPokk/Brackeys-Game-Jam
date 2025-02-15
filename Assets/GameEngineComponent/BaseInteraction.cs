using Engin.Utility;

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