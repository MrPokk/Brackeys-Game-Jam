
    public class ButtonNextTutoriaInfo : CustomButton
    {
        public override void Click() => InteractionCache<TutorialInfo>.AllInteraction.ForEach(manager => manager.Update() );
    }

