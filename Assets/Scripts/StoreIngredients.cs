using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class StoreIngredients : MonoBehaviour
{
    [SerializeField] public List<Ingredient> TilesList = new();
    public float Spacing;
    public bool AxisSwap;

    [Range(0.1f,2)]
    public float MaxSpacing;
    [Range(1, 20)]
    public float MaxSize;

    public virtual void Add(Ingredient Ingredient)
    {
        if (Ingredient == null) return;
        TilesList.Add(Instantiate(Ingredient, transform.position, new Quaternion()));
        TilesList.Sort();
        SetPoseTile();
    }
    public void Move(Ingredient Ingredient)
    {
        if (Ingredient == null) return;
        TilesList.Add(Ingredient);
        TilesList.Sort();
        SetPoseTile();
    }
    public virtual void Remove(Ingredient Ingredient)
    {
        TilesList.Remove(Ingredient);
        SetPoseTile();
    }
    public void Delete(Ingredient Ingredient)
    {
        TilesList.Remove(Ingredient);
        Destroy(Ingredient.gameObject);
        SetPoseTile();
    }
    public void DeleteAll()
    {
        foreach (var Ingredient in TilesList) {
            if (Ingredient.gameObject) Destroy(Ingredient.gameObject);
        }
        TilesList.Clear();
    }
    public bool Contains(Ingredient ingredient)
    {
        return TilesList.Contains(ingredient);
    }
    protected void SetPoseTile()
    {
        float size = MaxSpacing * TilesList.Count;
        if (size < MaxSize) Spacing = MaxSpacing;
        else {
            Spacing = MaxSize / size * MaxSpacing;
        }
        for (int i = 0; i < TilesList.Count; i++)
        {
            var space = new Vector3(0, Spacing * (i - TilesList.Count / 2f),0);
            if (AxisSwap) space = new Vector3(space.y, space.x,space.z);

            Vector3 Pose = gameObject.transform.position + space;
            Pose += Vector3.forward * (0.01f * i);
            TilesList[i].transform.DOMove(Pose, Main.AnimationMoveTime).SetEase(Ease.InOutElastic);

        }
    }
}
