using DG.Tweening;
using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class ButtonSack : CustomButton
{
    private SpriteRenderer spriteRenderer;
               public string Name;
    [TextArea] public string Description;
    public static float MoveSpeed = 2;
    public static bool BuyAnim = false;
    public static Vector3 Shift = new Vector3 (0, 3, 0);
    public int count = 1;
    public int countCatalist = 1;
    public int price = 10;
    public void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Buy()
    {
        BuyAnim = true;
        StartCoroutine(GenerateAll());
    }
    private int GetRandMin()
    {
        var Ingredients = CMS.Get<AllIngredients>().Ingredients.Where(x => x is Catalyst == false).ToList();
        Dictionary<int, int> countIngredients = new();
        foreach (var item in Ingredients)
        {
            countIngredients.Add(item.ID, 0);
        }

        var store = GameData<Main>.Boot.Store.TilesList;
        foreach (var item in store)
        {
            if (item is Catalyst == false)
            {
                countIngredients[item.ID] += 1;
            }
        }

        List<int> min = new();
        int minCount = int.MaxValue;
        foreach (var item in countIngredients)
        {
            if (item.Value < minCount)
            {
                minCount = item.Value;
            }
        }

        foreach (var item in countIngredients)
        {
            if (item.Value == minCount)
            {
                min.Add(item.Key);
            }
        }

        return min[Random.Range(0, min.Count)];
    }
    public IEnumerator GenerateAll()
    {
        Vector3 oldPos = transform.position;
        transform.DOMove(GameData<Main>.Boot.Store.gameObject.transform.position + Shift, MoveSpeed).SetEase(Ease.InOutElastic);
        yield return new WaitForSeconds(MoveSpeed);

        int oldSortingOrder = spriteRenderer.sortingOrder;
        spriteRenderer.sortingOrder = 0;

        for (int i = 0; i < count; i++)
        {
            yield return Generate(GetRandMin());
        }
        for (int i = 0; i < countCatalist; i++)
        {
            yield return Generate(CMS.Get<AllIngredients>().Ingredients.Find(x => x is Catalyst == true).ID);
        }

        spriteRenderer.sortingOrder = oldSortingOrder;
        transform.DOMove(oldPos, MoveSpeed).SetEase(Ease.InOutElastic);
        yield return new WaitForSeconds(MoveSpeed);

        BuyAnim = false;
        yield break;
    }
    public IEnumerator Generate(int IDingredient)
    {
        Ingredient ingredient = Instantiate(CMS.Get<AllIngredients>().GetByID(IDingredient), transform.position, new Quaternion());
        GameData<Main>.Boot.Store.Move(ingredient);
        SoundManager.PlaySound(SoundType.OpenPopup);
        yield return new WaitForSeconds(.5f);
        yield break;
    }
    public override void Click()
    {
        if (GameData<Main>.Money >= price && BuyAnim == false)
        {
            Buy();
            GameData<Main>.Money -= price;
        }
    }
}