using System;
using System.Collections.Generic;
using Engin.Utility;


public abstract class CMSEntity
{
    public List<IComponent> Components { get; set; } = new();
    public virtual ref T Define<T>(out T Component) where T : struct, IComponent
    {
        T RefComponent = new(); 
        Component = RefComponent; 
        return ref Component;
    }
    public abstract void RegisterComponents(params IComponent[] components);
    public virtual T Get<T>(out T RefComponent) where T : struct, IComponent
    {
        foreach (var Element in Components) {
            if (Element is T Component)
                return RefComponent = Component;
        }

        throw new Exception("Component not found");
    }
}