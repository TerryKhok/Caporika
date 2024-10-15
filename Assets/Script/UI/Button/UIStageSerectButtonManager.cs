using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStageSerectButtonManager : MonoBehaviour
{
    private GameObject stageButton;
    private GameObject irisObject;
    private Animator buttonAnim;

    
    void Start()
    {
        stageButton = GameObject.Find("StageButton");  
        buttonAnim = stageButton.GetComponent<Animator>();
        irisObject = GameObject.Find("IrisCanv");
    }

    /**
     * @brief 右→スクロール用ボタンを押されたときにアニメーションを再生する
     */
    public void RightButtonFunction()
    {
        buttonAnim.Play("RightbuttonScroll"); 
    }

    /**
     * @brief 左←スクロール用ボタンを押されたときにアニメーションを再生する
     */    
    public void LeftButtonFunction()
    {
        buttonAnim.Play("LeftbuttonScroll");
    }

    /**
     * @brief ステージセレクト用関数
     * @memo インスペクターでシーン名を代入
     */  
    public void StageSerectButton(string _str)
    {
        UIIrisScript iris = irisObject.GetComponent<UIIrisScript>();
        iris.IrisOut(_str); //次のシーンを代入
    }
}
