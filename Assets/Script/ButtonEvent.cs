using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvent : MonoBehaviour
{
    public GameObject obj;

    public void OnButtonPressed()
    {
        if (obj != null)
        {
            // •\Ž¦
            obj.SetActive(true);
        }
    }

    public void OnButtonRelease()
    {
        // ”ñ•\Ž¦
        obj.SetActive(false);
    }
}
