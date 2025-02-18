using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cauldron : MonoBehaviour
{
    public List<ObjectIngredient> ingredients = new List<ObjectIngredient>();
    public EffectsMaster effectsMaster = new EffectsMaster();
    public GameObject Potion;
    private Animator Animator => GetComponent<Animator>();

    private void Update()
    {
        if (ingredients.Any())
        {
            Animator.SetBool("IsShake", true);
        }
        else
        {
            Animator.SetBool("IsShake", false);
        }
    }

    public void Add(ObjectIngredient ingredient)
    {
        ingredients.Add(ingredient);
        ingredient.Prefab.GetComponent<Collider2D>().enabled = false;
        effectsMaster.AddEffects(ingredient.Ingredient.Effects);

        transform.DOPunchScale(new(GameData<Main>.Boot.AnimationScale, GameData<Main>.Boot.AnimationScale, 0), GameData<Main>.Boot.AnimationScaleTime, 0, 0);
    }
    private delegate void ResetUnlock();

    public bool Near(Vector2 pos)
    {
        return Vector2.Distance(pos, this.transform.position) < 1;
    }
    public bool Cook()
    {
        if (ingredients.Count == 0) return false;
        Instantiate(Potion, this.gameObject.transform.position, Quaternion.identity).GetComponent<Potion>().effects = effectsMaster.GetAndClear();
        foreach (ObjectIngredient @object in ingredients)
        {
            Destroy(@object.Prefab);
        }
        ingredients.Clear();

        transform.DOComplete();
        transform.DOPunchScale(new(GameData<Main>.Boot.AnimationScale, GameData<Main>.Boot.AnimationScale, 0), GameData<Main>.Boot.AnimationScaleTime, 0, 0);

        return true;
    }
}
