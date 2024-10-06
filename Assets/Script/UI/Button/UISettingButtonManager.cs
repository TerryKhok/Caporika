using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettingButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject keyBindCanvObj;  //KeyBindButtonをインスペクターで入れる
    [SerializeField] private GameObject SoundCanvObj;    //SoundButtonをインスペクターで入れる
    [SerializeField] private GameObject DisplayCanvObj;  //DisplayButtonをインスペクターで入れる

    private Canvas KeyBindCanvas;
    private Canvas SoundCanvas;
    private Canvas DisplayCanvas;

    public void Start()
    {
        KeyBindCanvas = keyBindCanvObj.GetComponent<Canvas>();
        SoundCanvas = SoundCanvObj.GetComponent<Canvas>();
        DisplayCanvas = DisplayCanvObj.GetComponent<Canvas>();
    }
    
    public void KeyBindButtonEvent()
    {
        KeyBindCanvas.enabled = true;
        SoundCanvas.enabled = false;
        DisplayCanvas.enabled = false;
    }

    public void SoundButtonEvent()
    {
        KeyBindCanvas.enabled = false;
        SoundCanvas.enabled = true;
        DisplayCanvas.enabled = false;
    }

    public void DisplayButtonEvent()
    {
        KeyBindCanvas.enabled = false;
        SoundCanvas.enabled = false;
        DisplayCanvas.enabled = true;
    }
}
