using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class UIIrisScript : MonoBehaviour
{

    //全部inspectorでオブジェクトを入れる
    public GameObject iris;         //irisを入れる
    public bool startIrisIn;
    //========================================================================

    public string nextScene;    //ボタンから送られてくる文字列を入れる用

    private Animator irisAnim; //アイリスアウト用
    private Canvas irisCanv;
    private CanvasGroup irisCanvasGroup;


    void Start()
    {
        irisAnim = iris.GetComponent<Animator>();
        irisCanv = this.GetComponent<Canvas>();
        irisCanvasGroup = this.GetComponent<CanvasGroup>();

        if (startIrisIn == true)
        {
            IrisIn();//アイリスインを再生
        }
        else
        {
            irisCanv.enabled = false;
        }

    }

    /**
     * @brief アイリスインを再生
     */
    public void IrisIn()
    {
        irisCanv.enabled = true;
        irisAnim.Play("IrisIn");    //アイリスインを再生

        irisCanvasGroup.interactable = false;  // 操作不能にする
        irisCanvasGroup.blocksRaycasts = false; // クリックなどのイベントを受け付けなくする
    }

    /**
     * @brief アイリスアウトを再生、引数のシーンに移動
     * 
     * @memo 
     */
    public void IrisOut(string _scene)
    {
        irisCanv.enabled = true;     //アイリスキャンバスをアクティブ化
        irisAnim.Play("IrisOut");       //アイリスアウトを再生
        nextScene = _scene;
    }
}
