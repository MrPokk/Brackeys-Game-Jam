using Engin.Utility;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public abstract class BasePeople : CMSEntity
{
    public DataPeople Data { get; set; }
    
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
}
public struct DataPeople : IComponent
{
    public GameObject Prefab;
    public GameObject TypePoison;
    public TMP_Text Description;
    public TMP_Text Name;
}
