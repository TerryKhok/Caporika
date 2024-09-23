using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimStageSerect : MonoBehaviour
{ 
    public GameObject leftbt;   //←スクロールボタンを入れる
    public GameObject rightbt;  //→スクロールボタンを入れる

    /**
     * @brief スクロール用のボタンを消す
     * 
     * @memo 消さんと途中で押されたら変な挙動なってまう
     */
    public void ButtonDelete()
    {
        leftbt.SetActive(false);
        rightbt.SetActive(false);
    }

    /**
     * @brief 右→スクロール用ボタンを押されたときにボタンを切り替える関数
     */
    public void RightButtonEvent()
    {
        leftbt.SetActive(true);
        rightbt.SetActive(false);
    }    

    /**
     * @brief 左←スクロール用ボタンを押されたときにボタンを切り替える関数
     */
    public void LeftButtonEvent()
    {
        leftbt.SetActive(false);
        rightbt.SetActive(true);
    }
}
