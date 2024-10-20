using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



/**
* @brief 設定変更画面で使うスクリプト
* @memo 設定のジャンルをボタンを押して変更するときに使う
*/   
public class UISettingButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject SoundCanvObj;    //SoundButtonをインスペクターで入れる
    [SerializeField] private GameObject DisplayCanvObj;  //DisplayButtonをインスペクターで入れる

    private Canvas SoundCanvas;
    private Canvas DisplayCanvas;

/**
* @brief 
* @memo 
*/   
    public void Start()
    {
        SoundCanvas = SoundCanvObj.GetComponent<Canvas>();
        DisplayCanvas = DisplayCanvObj.GetComponent<Canvas>();
    }

/**
* @brief Soundのボタンが押されたときに他二つを非表示にする
* @memo 
*/   
    public void SoundButtonEvent()
    {
        SoundCanvas.enabled = true;
        DisplayCanvas.enabled = false;
    }

/**
* @brief Displayのボタンが押されたときに他二つを非表示にする
* @memo 
*/   
    public void DisplayButtonEvent()
    {
        SoundCanvas.enabled = false;
        DisplayCanvas.enabled = true;
    }
}
