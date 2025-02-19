using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class StoreIngredients : MonoBehaviour
{
    [SerializeField] private List<GameObject> TilesList = new();
    public float Spacing;

    public void Add(ObjectIngredient Ingredient)
    {
        if (Ingredient.Prefab == null) return;
        TilesList.Add(Instantiate(Ingredient.Prefab,transform));
        SetPoseTile();
    }
    public void Move(GameObject Ingredient)
    {
        if (Ingredient == null) return;
        TilesList.Add(Ingredient);
        SetPoseTile();
    }
    public void Remove(GameObject Ingredient)
    {
        TilesList.Remove(Ingredient);
        SetPoseTile();
    }
    public void Delete(GameObject Ingredient)
    {
        TilesList.Remove(Ingredient);
        Destroy(Ingredient);
        SetPoseTile();
    }

    private void SetPoseTile()
    {
        for (int i = 0; i < TilesList.Count; i++)
        {
            Vector3 Pose = (Vector2)gameObject.transform.position + Vector2.down * Spacing * i - Vector2.down * Spacing * TilesList.Count / 2;
            Pose += Vector3.back * 0.01f * i;
            TilesList[i].transform.DOMove(Pose, GameData<Main>.Boot.AnimationMoveTime).SetEase(Ease.InOutElastic);

        }
    }
}
