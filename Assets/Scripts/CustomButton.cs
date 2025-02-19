using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class CustomButton : MonoBehaviour
{
    public void EventClose()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
