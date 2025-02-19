using System.Collections.Generic;
using UnityEngine;

public class Potion : Raise
{
    public int ID;
    public string Name;
    public int Price;
    public List<EffectData> effects = new();
    public List<int> IDIngredients = new();
    
    public void Set(SamplePotion potion, List<EffectData> effectData, List<int> IDIngredients)
    {
        ID = potion.ID;
        Name = potion.Name;
        Price = potion.Price;
        effects = effectData;
        this.IDIngredients = IDIngredients;

        SpriteRenderer sp = gameObject.GetComponent<SpriteRenderer>();
        SpriteRenderer spPotion = potion.GetComponent<SpriteRenderer>();
        sp.sprite = spPotion.sprite;
        sp.color = spPotion.color;
    }
}