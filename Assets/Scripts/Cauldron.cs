using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
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
        if (ingredient.Ingredient is Catalyst catalyst) {
            catalyst.Effect(effectsMaster.Get());
        }    }

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
        transform.DOComplete();
        transform.DOPunchScale(new(GameData<Main>.Boot.AnimationScale, GameData<Main>.Boot.AnimationScale, 0), GameData<Main>.Boot.AnimationScaleTime, 0, 0);        List<EffectData> effects = effectsMaster.GetAndClear();
      
        SamplePotion sample = CMS.Get<AllPotion>().GetAtEffects(effects);
        Potion potion = Instantiate(Potion, transform.position,new Quaternion()).GetComponent<Potion>();
        potion.Set(sample, effects, _ingredients);        
        return true;
    }
}
