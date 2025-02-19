using UnityEngine;

public class ButtonBell : CustomButton
{
    [Range(2, 10)]
    public int MinGoods;
    [Range(2, 10)]
    public int MaxGoods;
    public override void Click()
    {
        BasePeople people = PeopleImplementation.Customer;
        if (people != null) {
            if (people.DataComponent.Type == TypePeople.Customer) {
                PotionZone potionZone = GameData<Main>.Boot.PotionZone;
                if (potionZone.PotionIn != null) {
                    Potion potion = potionZone.PotionIn.GetComponent<Potion>();
                    if (potion.ID == people.DataComponent.TypePoison.ID) {
                        GameData<Main>.Boot.AddMoney(potion.Price);
                        potionZone.Delete();
                    }
                }
                //уходит
            }
            else {
                GameData<Main>.Boot.Shop.Generatre(Random.Range(MinGoods, MaxGoods + 1));
            }
        }
    }
}
