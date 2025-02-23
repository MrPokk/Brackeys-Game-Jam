using SmallHedge.SoundManager;
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
        if (GameData<Main>.Boot.Shop.isActiveAndEnabled) return;
        BasePeople people = PeopleImplementation.Customer;

        Animator.SetBool("IsClick", true);
        SoundManager.PlaySound(SoundType.ClickBell);

        if (people != null && PeopleImplementation.IsServiced)
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
                        int a = (int)PeopleImplementation.Customer.DataComponent.TypePoison.Difity * 20;
                        GameData<Main>.Money += a;
                        GameData<Main>.Reputation += a * 0.4f;
                        potionZone.Delete();
                        PeopleImplementation.ExitAll();
                        
                        
                        
                        return;
                    }
                }
                
                GameData<Main>.Reputation -= Main.ReputationDebuff;
                FileWriter.Write(people, null);
                PeopleImplementation.ExitAll();
            }
            else if (people.DataComponent.Type == TypePeople.Trader && PeopleImplementation.IsServiced)
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
