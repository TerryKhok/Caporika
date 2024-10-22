using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief   �����炭�f�o�b�O�p�̃X�N���v�g
 */
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
