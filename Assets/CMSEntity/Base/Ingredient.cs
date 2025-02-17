
    using System;
    using Engin.Utility;
    using UnityEngine;

    public abstract class Ingredient : CMSEntity
    {
        public DataIngredient Data;
    }


    public struct DataIngredient : IComponent
    {
        public GameObject Prefab;
    }
