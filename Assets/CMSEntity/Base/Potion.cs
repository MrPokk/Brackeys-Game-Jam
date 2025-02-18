using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour, IRaise
{
    public int ID;
    public List<EffectData> effects = new();
    public List<int> IDIngredients = new();

    public void Set(List<EffectData> effects, List<int> IDIngredients)
    {
        this.effects = effects;
        this.IDIngredients = IDIngredients;
    }
}