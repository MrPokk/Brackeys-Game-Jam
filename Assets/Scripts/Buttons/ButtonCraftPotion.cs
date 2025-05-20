
    using System;
    public class ButtonCraftPotion : CustomButton
    {
        
        public override void Click()
        {
            GameData<Main>.Boot.Cauldron.Cook();
        }
    }

