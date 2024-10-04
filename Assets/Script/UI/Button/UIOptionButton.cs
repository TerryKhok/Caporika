using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIOptionButton : MonoBehaviour
{
    private GameObject irisObject;
    private GameObject OptionCanvas;
    private Canvas OptionCanv;

    public void Start()
    {
        irisObject = GameObject.Find("IrisCanv");
        OptionCanvas = GameObject.Find("OptionCanv");
        OptionCanv = OptionCanvas.GetComponent<Canvas>();
    }

    /**
     * @brief コンテニューボタン用
     * @memo PoseCanvasEdn関数を呼び出す
     */
    public void ContinueButton()
    {
        SystemPose poseCanvas= GameObject.Find ("PoseCanvas").GetComponent<SystemPose>();
        poseCanvas.PoseEnd();
    }

    /**
     * @brief Optionボタン用
     * @memo オプションキャンバスを表示する
     */
    public void OptionButton()
    {
        OptionCanv.enabled = true;  //OptionCanvasを表示
    }

    /**
     * @brief ReStartボタン用
     * @memo 現在のシーン名を取得後、IrisOut関数にシーン名を渡す
     */
    public void RestartButton()
    {
        PoseFinish();
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
        PoseFinish();
        UIIrisScript iris = irisObject.GetComponent<UIIrisScript>();
        iris.IrisOut("TitleScene"); //次のシーンを代入

    }

    /**
     * @brief ステージセレクトボタン用
     * @memo IrisOut関数にStageSelectを渡す
     */
    public void StageSelectButton()
    {
        PoseFinish();
        UIIrisScript iris = irisObject.GetComponent<UIIrisScript>();
        iris.IrisOut("StageSelect"); //次のシーンを代入
    }

    /**
     * @brief ポーズに戻るボタン
     * @memo 
     */
    public void OptionReturn()
    {
        OptionCanv.enabled = false;  //OptionCanvasを非表示
    }

    public void PoseFinish()
    {
        SystemPose poseCanvas= GameObject.Find ("PoseCanvas").GetComponent<SystemPose>();
        poseCanvas.PoseEnd();
        OptionCanv.enabled = false;  //OptionCanvasを非表示
    }
}
