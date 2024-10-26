using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
* @brief ポーズ用クラス
* @memo 現在のシーン名を取得後、IrisOut関数にシーン名を渡す
*/
public class SystemPose : MonoBehaviour
{
    public KeyCode  PoseKey = KeyCode.Escape;          // Poseを行うキー
    bool is_poseNow = false;
    private Canvas poseCanvas;
    private CanvasGroup optionCanvasGroup;
    private GameObject OptionCanvas;
    private Canvas OptionCanv;
    
    void Start()
    {
        poseCanvas = this.GetComponent<Canvas>();
        poseCanvas.enabled = false;
        OptionCanvas = GameObject.Find("OptionCanv");
        OptionCanv = OptionCanvas.GetComponent<Canvas>();
        optionCanvasGroup = OptionCanv.GetComponent<CanvasGroup>();

    }

    /**
     * @brief UpData
     * @memo Escキーが押されたらポーズを切りかえる
     */
    void Update()
    {
        if (Input.GetKeyDown(PoseKey))
        {
            if (is_poseNow == false)    //PoseFgがfalseなら時間を止める
            {
                PoseStart();//ポーズスタート
            }
            else
            {
                PoseEnd();//ポーズエンド
            }
        }
    }

/**
* @brief ポーズスタート関数
* @memo ポーズ用キャンバスの表示と時間の停止をしている。
*/   
    public void PoseStart()
    {
        poseCanvas.enabled = true;  
        is_poseNow = true;  //ポーズフラグをtrueに
        Time.timeScale = 0.0f;  //時間を止める
    }
/**
* @brief ポーズ終了関数
* @memo ポーズ用キャンバスの非表示化と時間の停止を解除している。
*/   
    public void PoseEnd()
    {
        poseCanvas.enabled = false;
        is_poseNow = false; //ポーズフラグをfalseに
        OptionCanv.enabled = false;
        Time.timeScale = 1.0f;  //時間を進める
        optionCanvasGroup.interactable = false;  // 操作不能にする
        optionCanvasGroup.blocksRaycasts = false; // クリックなどのイベントを受け付けなくする
    }

}
