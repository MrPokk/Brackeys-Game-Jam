using System;
using System.Collections.Generic;
using Engin.Utility;

public static class CMS
{
    private static List<CMSEntity> CMSEntities = new();
    public static void Init()
    {
        FindAll();
    }

    private static void FindAll()
    {
        var ListCMSEntity = ReflectionUtility.FindAllImplement<CMSEntity>();
        foreach (var Element in ListCMSEntity) {
            CMSEntities.Add(Activator.CreateInstance(Element) as CMSEntity);
        }
    }

    public static T Get<T>() where T : CMSEntity
    {
        foreach (var Element in CMSEntities) {
            if (Element is T ElementData)
                return ElementData;
        }
        throw new Exception("CMSEntity not found");
    }
    public static List<T> GetAll<T>() where T : CMSEntity
    {
        List<T> list = new List<T>();
        foreach (var Element in CMSEntities) {
            if (Element is T ElementData)
                list.Add(ElementData);
        }
        if (list.Count == 0)
            throw new Exception("CMSEntity not found");
        return list;
    }
}