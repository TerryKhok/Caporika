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
    [SerializeField] private GameObject keyBindCanvObj;  //KeyBindButtonをインスペクターで入れる
    [SerializeField] private GameObject SoundCanvObj;    //SoundButtonをインスペクターで入れる
    [SerializeField] private GameObject DisplayCanvObj;  //DisplayButtonをインスペクターで入れる

    private Canvas KeyBindCanvas;
    private Canvas SoundCanvas;
    private Canvas DisplayCanvas;

/**
* @brief 
* @memo 
*/   
    public void Start()
    {
        KeyBindCanvas = keyBindCanvObj.GetComponent<Canvas>();
        SoundCanvas = SoundCanvObj.GetComponent<Canvas>();
        DisplayCanvas = DisplayCanvObj.GetComponent<Canvas>();
    }
    
/**
* @brief KeyBindのボタンが押されたときに他二つを非表示にする
* @memo 
*/   
    public void KeyBindButtonEvent()
    {
        KeyBindCanvas.enabled = true;
        SoundCanvas.enabled = false;
        DisplayCanvas.enabled = false;
    }

/**
* @brief Soundのボタンが押されたときに他二つを非表示にする
* @memo 
*/   
    public void SoundButtonEvent()
    {
        KeyBindCanvas.enabled = false;
        SoundCanvas.enabled = true;
        DisplayCanvas.enabled = false;
    }

/**
* @brief Displayのボタンが押されたときに他二つを非表示にする
* @memo 
*/   
    public void DisplayButtonEvent()
    {
        KeyBindCanvas.enabled = false;
        SoundCanvas.enabled = false;
        DisplayCanvas.enabled = true;
    }
}
