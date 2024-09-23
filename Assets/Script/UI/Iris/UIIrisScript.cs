using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class UIIrisScript : MonoBehaviour
{
    bool irisInFg = false;  //アイリスインをしたかフラグ
    bool irisOutFg = false; //アイリスアウトをしたかフラグ

    //全部inspectorでオブジェクトを入れる
    public GameObject  irisCanvas;  //irisCanvasを入れる
    public GameObject iris;         //irisを入れる
    public GameObject lid;          //irisLibを入れる
    //==================================

    public string nextScene;    //ボタンから送られてくる文字列を入れる用

    private Animator irisAnim; //アイリスアウト用


    void Start()
    {
        irisAnim = iris.GetComponent<Animator>();
        IrisIn();//アイリスインを再生
    }

    /**
     * @brief アイリスインを再生
     */
    public void IrisIn()
    {
        if(irisInFg == false)
        {
            irisCanvas.SetActive(true);
            irisAnim.Play("IrisIn");    //アイリスインを再生
        }
    }

    /**
     * @brief アイリスアウトを再生、引数のシーンに移動
     * 
     * @memo 
     */
    public void IrisOut(string _scene)
    {
        if(irisOutFg == false)
        {
            irisCanvas.SetActive(true);     //アイリスキャンバスをアクティブ化
            irisAnim.Play("IrisOut");       //アイリスアウトを再生
            nextScene = _scene;
        }
    }
}
