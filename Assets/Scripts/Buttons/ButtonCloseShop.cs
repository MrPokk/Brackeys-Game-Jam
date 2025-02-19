public class ButtonCloseShop : CustomButton
{
    public override void Click() => EventClose();

    public void EventClose()
    {
        //уходит
        transform.parent.gameObject.SetActive(false);
    }
}
