public class ButtonCloseShop : CustomButton
{
    public override void Click() => EventClose();

    public void EventClose()
    {
        //������
        transform.parent.gameObject.SetActive(false);
    }
}
