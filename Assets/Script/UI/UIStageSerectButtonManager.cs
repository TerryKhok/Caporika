using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStageSerectButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject stageButton;
    
    private Animator buttonAnim;

    
    void Start()
    {
        buttonAnim = stageButton.GetComponent<Animator>();  
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
}
