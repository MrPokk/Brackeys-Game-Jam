using UnityEngine;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
class MyDebug : BaseInteraction, IEnterInUpdate
{
    void IEnterInUpdate.Update(float TimeDelta)
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            var LicoriceRoot = CMS.Get<AllIngredients>().Ingredients[Random.Range(0, CMS.Get<AllIngredients>().Ingredients.Count)];
            GameData<Main>.Boot.Store.Add(LicoriceRoot);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            var All = CMS.Get<AllIngredients>().Ingredients;
            foreach (var Element in All)
                GameData<Main>.Boot.Store.Add(Element);
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            GameData<Main>.Boot.Shop.Generatre(Random.Range(3, 7));
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            var list = CMS.Get<AllPotion>().PotionsPull;
            FileWriter.Write(list);
        }
    }

    public static SamplePotion CustomCustomerPotion(int IDPotion)
    {
        return CMS.Get<AllPotion>().GetByID(IDPotion);
    }
}
#endif