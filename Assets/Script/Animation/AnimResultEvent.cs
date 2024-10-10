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

    /**
     * @brief リザルトアニメーションの最初に呼び出されるイベント
     */  
    public void ResultAnimStart()
    {
        resultCanv.enabled = true;  //リザルトキャンバスを表示
    }

    /**
     * @brief リザルトアニメーションの最後に呼び出されるイベント
     */  
    public void ResultAnimFinish()
    {
        titleButton.interactable = true;    //ボタンにインタラクトできるようにする
        nextButton.interactable = true;     //ボタンにインタラクトできるようにする
    }
}
