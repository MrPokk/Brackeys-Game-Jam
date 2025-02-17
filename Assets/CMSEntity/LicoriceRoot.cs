
    using Engin.Utility;
    using UnityEngine;

    public class LicoriceRoot : Ingredient
    {
       public LicoriceRoot()
        {
            Define<DataIngredient>(out var component).Prefab = Resources.Load<GameObject>("Assets/Prefab/LicoriceRoot.prefab");
            RegisterComponents(component);
        }

        public override void RegisterComponents(params IComponent[] components)
        {
            Components.AddRange(components);
        }
    }
    
