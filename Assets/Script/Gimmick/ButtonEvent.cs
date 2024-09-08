using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief   おそらくデバッグ用のスクリプト
 */
public class ButtonEvent : MonoBehaviour
{
    public GameObject obj;

    public void OnButtonPressed()
    {
        if (obj != null)
        {
            // 表示
            obj.SetActive(true);
        }
    }

    public void OnButtonRelease()
    {
        // 非表示
        obj.SetActive(false);
    }
}
