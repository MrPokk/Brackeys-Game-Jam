using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ButtonBell : CustomButton
{
    [Range(2, 16)]
    public int MinGoods;
    [Range(2, 16)]
    public int MaxGoods;

    private Animator Animator;
    private void Start()
    {
        Animator = GetComponent<Animator>();
    }
    public override void Click()
    {
        BasePeople people = PeopleImplementation.Customer;

        Animator.SetBool("IsClick", true);
        
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

    public void ResetTrigger()
    {
        Animator.SetBool("IsClick", false);
    }
}
