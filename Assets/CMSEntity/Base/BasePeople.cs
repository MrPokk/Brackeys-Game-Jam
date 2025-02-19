using Engin.Utility;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public abstract class BasePeople : CMSEntity
{
    public DataPeople DataComponent { get; set; }
    public virtual void SetTextPrefab(ref DataPeople Data)
    {
        foreach (var Element in Data.Prefab.GetComponentsInChildren<TMP_Text>())
        {
            if (Element.name == "Name")
            {
                Data.Name = Element;
            }
            else if (Element.name == "Description")
            {
                Data.Description = Element;
            }
        }
    }
    
    public abstract void SetData(ref DataPeople Data);
}
public struct DataPeople : IComponent
{
    public GameObject Prefab;
    public SamplePotion TypePoison;
    public TMP_Text Description;
    public TMP_Text Name;
    public TypePeople Type;
}
public enum TypePeople
{
    Customer,
    Trader
}