using DG.Tweening;
using SmallHedge.SoundManager;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour
{
    public List<Ingredient> ingredients = new List<Ingredient>();
    public EffectsMaster effectsMaster = new EffectsMaster();
    public GameObject Potion;

    public GameObject CraftButton;
    
    private Animator Animator;
    private AllPotion AllPotion;

    private const float SIZE_INFO = 1.2f;
    [SerializeField] private Transform CenterOfNeck;
    private float Spase = 0;
    private float Offset = 0;
    public Vector2 SizeOrbit = new Vector2(1, 1);

    private void Start()
    {
        AllPotion = CMS.Get<AllPotion>();
        Animator = GetComponent<Animator>();
    }
    private void Update()
    {
        Offset += Time.deltaTime;
        for (int i = 0; i < ingredients.Count; i++) {
            float x = Mathf.Sin(Spase * i + Offset) * SizeOrbit.x;
            float y = Mathf.Cos(Spase * i + Offset) * SizeOrbit.y;
            float z = y / 10;
            ingredients[i].transform.position = CenterOfNeck.position + new Vector3(x, y, z);
        }
    }

    public void Add(Ingredient ingredient)
    {
        Animator.SetBool("IsShake", true);
        PotionInfo.OpenPopup<Cauldron>();
        
        CraftButton.gameObject.SetActive(true);
        
        ingredients.Add(ingredient);
        ingredient.GetComponent<SpriteRenderer>().sortingOrder = 0;
        ingredient.GetComponent<Collider2D>().enabled = false;
        effectsMaster.AddEffects(ingredient.Effects);
        
        SoundManager.PlaySound(SoundType.Gurgle);
        
        transform.DOPunchScale(new(Main.AnimationScale, Main.AnimationScale, 0), Main.AnimationScaleTime, 0, 0).OnComplete(() => {
            if (ingredient is Catalyst catalyst) {
                catalyst.Effect(effectsMaster.Get());
            }
            
            foreach (var Element in InteractionCache<PotionInfo>.AllInteraction)
                Element.UpdateInfo();
        });
        Spase = 2 * Mathf.PI / ingredients.Count;
    }
    public bool Near(Vector2 pos)
    {
        return Vector2.Distance(pos, CenterOfNeck.position) < 1.2;
    }
    public bool Cook()
    {
        Animator.SetBool("IsShake", false);
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
        CraftButton.gameObject.SetActive(false);
        

        List<EffectData> effects = effectsMaster.GetAndClear();
        SamplePotion sample = AllPotion.GetAtEffects(effects, PeopleImplementation.Customer.DataComponent.TypePoison);
        // Если сварил говно
        if (sample == AllPotion.Bad) return false;

        Potion potion = Instantiate(Potion, transform.position + Vector3.up, new Quaternion()).GetComponent<Potion>();
        potion.Set(sample, effects, _ingredients);
        
        return true;
    }
    public SamplePotion GetEffectPotion()
    {
        return AllPotion.GetAtEffects(effectsMaster.Get(), PeopleImplementation.Customer.DataComponent.TypePoison);
    }
}
