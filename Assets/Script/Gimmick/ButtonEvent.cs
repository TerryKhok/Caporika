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
            // �\��
            obj.SetActive(true);
        }
    }

    public void OnButtonRelease()
    {
        // ��\��
        obj.SetActive(false);
    }
}
