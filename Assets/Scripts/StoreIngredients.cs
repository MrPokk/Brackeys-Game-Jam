using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class StoreIngredients : MonoBehaviour
{
    [SerializeField] private List<Ingredient> TilesList = new();
    public float Spacing;
    public bool AxisSwap;

    public void Add(Ingredient Ingredient)
    {
        if (Ingredient == null) return;
        TilesList.Add(Instantiate(Ingredient,transform.position, new Quaternion()));
        SetPoseTile();
    }
    public void Move(Ingredient Ingredient)
    {
        if (Ingredient == null) return;
        TilesList.Add(Ingredient);
        SetPoseTile();
    }
    public void Remove(Ingredient Ingredient)
    {
        TilesList.Remove(Ingredient);
        SetPoseTile();
    }
    public void Delete(Ingredient Ingredient)
    {
        TilesList.Remove(Ingredient);
        Destroy(Ingredient);
        SetPoseTile();
    }
    public bool Contains(Ingredient ingredient)
    {
        return TilesList.Contains(ingredient);
    }
    protected void SetPoseTile()
    {
        for (int i = 0; i < TilesList.Count; i++)
        {
            Vector2 space = new Vector2(0, Spacing * (i - TilesList.Count / 2f));
            if (AxisSwap) space = new Vector2(space.y, space.x);

            Vector3 Pose = (Vector2)gameObject.transform.position + space;
            Pose += Vector3.forward * (0.01f * i);
            TilesList[i].transform.DOMove(Pose, GameData<Main>.Boot.AnimationMoveTime).SetEase(Ease.InOutElastic);

        }
    }
}
