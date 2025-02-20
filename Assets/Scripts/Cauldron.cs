using DG.Tweening;
using SmallHedge.SoundManager;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class Cauldron : MonoBehaviour
{
    public List<Ingredient> ingredients = new List<Ingredient>();
    public EffectsMaster effectsMaster = new EffectsMaster();
    public GameObject Potion;

    private Animator Animator;
    private AllPotion AllPotion;

    private const float SIZE_INFO = 1.2f;
    
    private void Start()
    {
        AllPotion = CMS.Get<AllPotion>();
        Animator = GetComponent<Animator>();
    }
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

    public void Add(Ingredient ingredient)
    {

        PotionInfo.OpenPopup<Cauldron>();
        
        ingredients.Add(ingredient);
        ingredient.GetComponent<Collider2D>().enabled = false;
        effectsMaster.AddEffects(ingredient.Effects);
        transform.DOPunchScale(new(Main.AnimationScale, Main.AnimationScale, 0), Main.AnimationScaleTime, 0, 0);
        if (ingredient is Catalyst catalyst) {
            catalyst.Effect(effectsMaster.Get());
        }
        
        
        SoundManager.PlaySound(SoundType.Gurgle);
    }
    public bool Near(Vector2 pos)
    {
        return Vector2.Distance(pos, this.transform.position) < 1;
    }
    public bool Cook()
    {
        if (ingredients.Count == 0) return false;
        List<int> _ingredients = new List<int>();
        foreach (Ingredient item in ingredients) {
            _ingredients.Add(item.ID);
            Destroy(item.gameObject);
        }
        ingredients.Clear();
        transform.DOComplete();
        transform.DOPunchScale(new(Main.AnimationScale, Main.AnimationScale, 0), Main.AnimationScaleTime, 0, 0);
        
        PotionInfo.ClosePopup<Cauldron>();

        List<EffectData> effects = effectsMaster.GetAndClear();
        SamplePotion sample = AllPotion.GetAtEffects(effects);
        // Если сварил говно
        if (sample == AllPotion.Bad) return false;

        Potion potion = Instantiate(Potion, transform.position + Vector3.up , new Quaternion()).GetComponent<Potion>();
        potion.Set(sample, effects, _ingredients);
        
        return true;
    }
    public SamplePotion GetEffectPotion()
    {
        return AllPotion.GetAtEffects(effectsMaster.Get());
    }
}
