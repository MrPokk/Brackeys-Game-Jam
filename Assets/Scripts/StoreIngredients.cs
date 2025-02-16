using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public partial class StoreIngredients : MonoBehaviour
{
    private List<Ingredient> TilesList = new();
    private float Spacing;

    private void Update()
    {
        SetPoseTile();
    }

    public void Add(Ingredient Ingredient)
    {
        Ingredient.Get<DataIngredient>(out var Component);
        Instantiate(Component.Prefab);
        TilesList.Add(Ingredient);
    }

    public void RemoveTile(Ingredient Ingredient)
    {
        TilesList.Remove(Ingredient);
    }

    public void DeleteTile(Ingredient Ingredient)
    {
        TilesList.Remove(Ingredient);
    }


    private void SetPoseTile()
    {
        for (int i = 0; i < TilesList.Count; i++)
        {
            var Offset = i * Spacing - TilesList.Count * Spacing;

            TilesList[i].Data.Prefab.transform.localPosition = new Vector2(0, i * Spacing) + Vector2.down * Offset;
            // var Tween = TilesList[i].CreateTween();
            //Tween.SetSpeedScale(GameData<Main>.Boot.ANIMATION_SPEED);
            // Tween.TweenProperty(TilesList[i], "position", new Vector2(0, i * Spacing) + Vector2.Down * Offset, 1).SetTrans(Tween.TransitionType.Linear);
        }
    }
}