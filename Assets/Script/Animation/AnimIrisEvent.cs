using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimIrisEvent : MonoBehaviour
{
    public GameObject IrisCanvas;
    private Canvas irisCanv;
    public bool irisInFinish = false;
    
    public void Start()
    {
        irisCanv = IrisCanvas.GetComponent<Canvas>();
    }

    /**
     * @brief アイリスアウト終了時のシーンチェンジイベント用
     * @memo  UiIrisScriptから次のシーンの名前を取ってきている
     */
    public void IrisOutSceneChangeEvent()
    {
        UIIrisScript iris =  IrisCanvas.GetComponent<UIIrisScript>(); 
        SceneManager.LoadScene(iris.nextScene); //引数にステージセレクトシーンを代入
    }

    /**
     * @brief アイリスインアニメーション中のイベント用
     * @memo アイリス用キャンバスを非表示に(しないとボタンが押せないため)
     */
    public void IrisInCanvasEvent()
    {
        irisCanv.enabled = false;
        irisInFinish = true;
    }
}
