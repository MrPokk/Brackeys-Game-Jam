using DG.Tweening;
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
          
            Potion.transform.DOMove(this.transform.position,GameData<Main>.Boot.AnimationMoveTime).SetEase(Ease.InOutElastic);
            SpritePotionZone.transform.DOPunchScale(new(GameData<Main>.Boot.AnimationScale, GameData<Main>.Boot.AnimationScale, 0), GameData<Main>.Boot.AnimationScaleTime, 0, 0);
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