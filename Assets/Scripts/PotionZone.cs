using DG.Tweening;
using SmallHedge.SoundManager;
using UnityEngine;

public class PotionZone : MonoBehaviour
{
    public GameObject PotionIn {get; private set; }
    public bool InZone { get; private set; } = false;

    [SerializeField] private GameObject SpritePotionZone;
    public bool Near(Vector2 pos)
    {
        return Vector2.Distance(pos, this.transform.position) < 1f;
    }

    public void Add(GameObject Potion)
    {
        if (Potion != null && InZone == false)
        {
            PotionIn = Potion;
            InZone = true;
          
            SoundManager.PlaySound(SoundType.DropPotionInSlot);
            
            Potion.transform.DOMove(this.transform.position,Main.AnimationMoveTime).SetEase(Ease.InOutElastic);
            SpritePotionZone.transform.DOPunchScale(new(Main.AnimationScale, Main.AnimationScale, 0), Main.AnimationScaleTime, 0, 0);
        }
    }

    public void Remove()
    {
        InZone = false;
        PotionIn = null;
    }
    public void Delete()
    {
        InZone = false;
        Destroy(PotionIn);
    }
}