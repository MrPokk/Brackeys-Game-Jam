
    public class ButtonCloseTutorialInfo : CustomButton
    {
        public override void Click()
        {
            TutorialInfo.TutorialComplete = true;
            InteractionCache<TutorialInfo>.AllInteraction.ForEach(manager => manager.Update());
        }
    }

