using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PotionZone : MonoBehaviour
{
    private GameObject PotionIn;
    public bool Near(Vector2 pos)
    {
        return Vector2.Distance(pos, this.transform.position) < 1;
    }

    public void Add(ObjectIngredient PotionInHand)
    {
        var Potion = PotionInHand.Prefab;
        if (Potion != null && PotionIn == null)
        {
            PotionIn = Potion;
          //Potion.GetComponent<Collider2D>().enabled = false;
            Potion.transform.DOMove(this.transform.position,0.3f).SetEase(Ease.InOutElastic);
        }
    }

    public void Remove()
    {
        PotionIn = null;
    }
}