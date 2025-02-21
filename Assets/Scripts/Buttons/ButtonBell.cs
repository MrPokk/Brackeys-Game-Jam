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
        if (people != null)
        {
            if (people.DataComponent.Type == TypePeople.Customer)
            {
                PotionZone potionZone = GameData<Main>.Boot.PotionZone;
                
                if (potionZone.PotionIn != null)
                {
                    Potion potion = potionZone.PotionIn.GetComponent<Potion>();
                    FileWriter.Write(people, potion);
                    if (potion.ID == people.DataComponent.TypePoison.ID)
                    {
                        GameData<Main>.Money += potion.Price;
                        GameData<Main>.Reputation += potion.Price * 0.1f;
                        potionZone.Delete();
                        PeopleImplementation.ExitAll();
                        
                        return;
                    }
                }
                
                GameData<Main>.Reputation -= Main.ReputationDebuff;
                FileWriter.Write(people, null);
                PeopleImplementation.ExitAll();
            }
            else if (people.DataComponent.Type == TypePeople.Trader)
            {
                GameData<Main>.Boot.Shop.Generatre(Random.Range(MinGoods, MaxGoods + 1));
            }
        }
    }
}
