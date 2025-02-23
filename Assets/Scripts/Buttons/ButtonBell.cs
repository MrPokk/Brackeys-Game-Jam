using DG.Tweening;
using SmallHedge.SoundManager;
using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ButtonBell : CustomButton
{
    private Animator Animator;
    
    private static bool IsPunchScale = false;
    private void Start()
    {
        Animator = GetComponent<Animator>();

    }

    public void Update()
    {
        if (!GameData<Main>.Boot.Store.TilesList.Any() && !IsPunchScale && !GameData<Main>.Boot.Cauldron.ingredients.Any())
        {
            IsPunchScale = true;
            transform.DOPunchScale(new(Main.AnimationScale  - 0.3f, Main.AnimationScale - 0.3f, 0), Main.AnimationScaleTime + 0.5f,0,0).OnComplete((() => {
                IsPunchScale = false;
            }));
        }
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
                        GameData<Main>.Reputation += a * 0.7f;
                        potionZone.Delete();
                        PeopleImplementation.ExitAll();
  
                        return;
                    }
                }
                
                GameData<Main>.Reputation -= Main.ReputationDebuff;
                FileWriter.Write(people, null);
                PeopleImplementation.ExitAll();
            }
        }
    }

    public void ResetTrigger()
    {
        Animator.SetBool("IsClick", false);
    }
}
