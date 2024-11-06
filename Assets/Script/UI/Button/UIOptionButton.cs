using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;


/**
* @brief ゲーム内のオプションで使用されているボタンたちにつけるスクリプト
* @memo 
*/
public class UIOptionButton : MonoBehaviour
{
    private GameObject irisObject;
    private GameObject OptionCanvas;
    private Canvas OptionCanv;
    CanvasGroup optionCanvasGroup;

    private GameObject soundOptionCanvas;
    private GameObject displayOptionCanvas;

    private Canvas soundCanvas;
    private Canvas displayCanvas;



    public void Start()
    {
        irisObject = GameObject.Find("IrisCanv");
        OptionCanvas = GameObject.Find("OptionCanv");

        soundOptionCanvas = GameObject.Find("SoundCanvas");
        soundCanvas = soundOptionCanvas.GetComponent<Canvas>();
        displayOptionCanvas = GameObject.Find("DisplayCanvas");
        displayCanvas = displayOptionCanvas.GetComponent<Canvas>();

        OptionCanv = OptionCanvas.GetComponent<Canvas>();

        //OptionCanvas.SetActive(false);  //OptionCanvasを表示
        OptionCanv.enabled = false;
        optionCanvasGroup = OptionCanv.GetComponent<CanvasGroup>();
        optionCanvasGroup.interactable = false;  // 操作不能にする
        optionCanvasGroup.blocksRaycasts = false; // クリックなどのイベントを受け付けなくする
    }

    /**
     * @brief コンテニューボタン用
     * @memo PoseCanvasEdn関数を呼び出す
     */
    public void ContinueButton()
    {
        SystemPose poseCanvas = GameObject.Find("PauseCanvas").GetComponent<SystemPose>();
        poseCanvas.PoseEnd();
    }

    /**
     * @brief Optionボタン用
     * @memo オプションキャンバスを表示する
     */
    public void OptionButton()
    {
        OptionCanv.enabled = true;

        soundCanvas.enabled = true;  //SoundCanvasを表示
        displayCanvas.enabled = false;  //SoundCanvasを表示

        optionCanvasGroup.interactable = true;  // 操作可能にする
        optionCanvasGroup.blocksRaycasts = true; // クリックなどのイベントを受け付ける
    }

    /**
     * @brief ReStartボタン用
     * @memo 現在のシーン名を取得後、IrisOut関数にシーン名を渡す
     */
    public void RestartButton()
    {
        PauseFinish();
        string currentSceneName = SceneManager.GetActiveScene().name;//現在アクティブなシーンの名前を取得
        UIIrisScript iris = irisObject.GetComponent<UIIrisScript>();
        iris.IrisOut(currentSceneName); //次のシーンを代入

    }

    /**
     * @brief タイトルボタン用
     * @memo IrisOut関数にTitleSceneを渡す
     */
    public void TitleButton()
    {
        PauseFinish();
        UIIrisScript iris = irisObject.GetComponent<UIIrisScript>();
        iris.IrisOut("TitleScene"); //次のシーンを代入

    }

    /**
     * @brief ステージセレクトボタン用
     * @memo IrisOut関数にStageSelectを渡す
     */
    public void StageSelectButton()
    {
        PauseFinish();
        UIIrisScript iris = irisObject.GetComponent<UIIrisScript>();
        iris.IrisOut("StageSelect"); //次のシーンを代入
    }

    /**
     * @brief 設定からポーズに戻るボタン
     * @memo 
     */
    public void OptionReturn()
    {
        optionCanvasGroup.interactable = false;  // 操作不能にする
        optionCanvasGroup.blocksRaycasts = false; // クリックなどのイベントを受け付けなくする

        soundCanvas.enabled = false;  //SoundCanvasを非表示
        displayCanvas.enabled = false;  //DisplayCanvasを非表示
        OptionCanv.enabled = false;
    }

    /**
    * @brief ポーズ状態で開かれているすべてのタブをを終了する関数
    * @memo Canvas閉じてPauseEnd関数読んでるだけ
*/
    public void PauseFinish()
    {
        SystemPose poseCanvas = GameObject.Find("PauseCanvas").GetComponent<SystemPose>();

        soundCanvas = soundOptionCanvas.GetComponent<Canvas>();
        displayCanvas = displayOptionCanvas.GetComponent<Canvas>();

        soundCanvas.enabled = false;  //SoundCanvasを非表示
        displayCanvas.enabled = false;  //DisplayCanvasを非表示
        OptionCanv.enabled = false;  //OptionCanvasを非表示
        poseCanvas.PoseEnd();
    }



}
