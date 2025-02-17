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
    }
    public bool Near(Vector2 pos)
    {
        return Vector2.Distance(pos, this.transform.position) < 1;
    }
    public bool Cook()
    {
        if (ingredients.Count == 0) return false;
        Instantiate(Potion, this.gameObject.transform.position, Quaternion.identity).GetComponent<Potion>().effects = effectsMaster.GetAndClear();
        foreach (ObjectIngredient @object in ingredients) {
            Destroy(@object.Prefab);
        }
        ingredients.Clear();
        return true;
    }
}