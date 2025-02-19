public class ButtonCloseShop : CustomButton
{
    public override void Click() => EventClose();

    public void EventClose()
    {
        PeopleImplementation.ExitAll();
        transform.parent.gameObject.SetActive(false);
    }
}
