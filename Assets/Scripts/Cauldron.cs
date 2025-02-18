using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour
{
    public List<ObjectIngredient> ingredients = new List<ObjectIngredient>();
    public EffectsMaster effectsMaster = new EffectsMaster();
    public GameObject Potion;

    public void Add(ObjectIngredient ingredient)
    {
        ingredients.Add(ingredient);
        ingredient.Prefab.GetComponent<Collider2D>().enabled = false;
        effectsMaster.AddEffects(ingredient.Ingredient.Effects);

        if (ingredient.Ingredient is Catalyst catalyst) {
            catalyst.Effect(effectsMaster.Get());
        }
    }
    public bool Near(Vector2 pos)
    {
        return Vector2.Distance(pos, this.transform.position) < 1;
    }
    public bool Cook()
    {
        if (ingredients.Count == 0) return false;
        List<int> _ingredients = new List<int>();
        foreach (ObjectIngredient item in ingredients) {
            _ingredients.Add(item.Ingredient.ID);
            Destroy(item.Prefab);
        }
        ingredients.Clear();

        List<EffectData> effects = effectsMaster.GetAndClear();
        SamplePotion sample = CMS.Get<AllPotion>().GetAtEffects(effects);
        Potion potion = Instantiate(Potion, transform).GetComponent<Potion>();
        potion.Set(sample, effects, _ingredients);
        return true;
    }
}