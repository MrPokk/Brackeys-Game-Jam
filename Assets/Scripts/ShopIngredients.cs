using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopIngredients : StoreIngredients
{

    public override void Remove(Ingredient Ingredient)
    {
        Ingredient.PriceText.gameObject.SetActive(false);
        base.Remove(Ingredient);
    }
    public override void Add(Ingredient Ingredient)
    {
        if (Ingredient == null) return;
        Ingredient = Instantiate(Ingredient, transform.position, new Quaternion());

        Ingredient.PriceText.text = Ingredient.Price.ToString();
        Ingredient.PriceText.gameObject.SetActive(true);
        TilesList.Add(Ingredient);
        SetPoseTile();
    }
}
