
    public class ButtomCraftPotion : CustomButton
    {
        public override void Click()
        {
            GameData<Main>.Boot.Cauldron.Cook();
        }
    }

