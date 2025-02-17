using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public partial class StoreIngredients : MonoBehaviour
{
    [SerializeField] private List<GameObject> TilesList = new();
    public float Spacing;

    private void Update()
    {
        SetPoseTile();
    }

    public void Add(Ingredient Ingredient)
    {
        Ingredient.Get<DataIngredient>(out var Component);
        TilesList.Add(Instantiate(Component.Prefab));
    }

    private void SetPoseTile()
    {
        for (int i = 0; i < TilesList.Count; i++)
        {
            var Offset = i * Spacing - TilesList.Count * Spacing;

            TilesList[i].transform.localPosition = Vector2.down * Offset * i;
            // var Tween = TilesList[i].CreateTween();
            //Tween.SetSpeedScale(GameData<Main>.Boot.ANIMATION_SPEED);
            // Tween.TweenProperty(TilesList[i], "position", new Vector2(0, i * Spacing) + Vector2.Down * Offset, 1).SetTrans(Tween.TransitionType.Linear);
        }
    }
}