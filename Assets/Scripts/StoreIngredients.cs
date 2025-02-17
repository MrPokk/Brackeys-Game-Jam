using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public partial class StoreIngredients : MonoBehaviour
{
    [SerializeField] private List<GameObject> TilesList = new();
    public float Spacing;

    private void Update()
    {
        //SetPoseTile();
    }

    public void Add(ObjectIngredient Ingredient)
    {
        if (Ingredient.Prefab == null) return;
        TilesList.Add(Instantiate(Ingredient.Prefab));
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
            
            var Pose = (Vector2)gameObject.transform.position + Vector2.down * Spacing * i;
            TilesList[i].transform.DOMove((Pose),0.5f).SetEase(Ease.InOutElastic);

        }
    }
}