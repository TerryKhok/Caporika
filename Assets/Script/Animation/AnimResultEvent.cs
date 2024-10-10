using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimResultEvent : MonoBehaviour
{
    public  GameObject resultCanvasObj;      //リザルトキャンバスをインスペクターで代入
    public GameObject titleButtonObj;   //タイトルボタンをインスペクターで代入
    public GameObject nextButtonObj;    //ネクストボタンをインスペクターで代入
    private Canvas resultCanv;
    private Button titleButton;         
    private Button nextButton;

    public void Start()
    {
        resultCanv = resultCanvasObj.GetComponent<Canvas>();
        titleButton = titleButtonObj.GetComponent<Button>();
        nextButton = nextButtonObj.GetComponent<Button>();
    }
    public void ResultAnimStart()
    {
        resultCanv.enabled = true;
    }
    public void ResultAnimFinish()
    {
        titleButton.interactable = true;
        nextButton.interactable = true;
    }
}
