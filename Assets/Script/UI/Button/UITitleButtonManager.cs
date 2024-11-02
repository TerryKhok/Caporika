using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonManager : MonoBehaviour
{
    private GameObject irisObject;

    private GameObject OptionCanvas;
    private GameObject SoundOptionCanvas;
    private GameObject DisplayOptionCanvas;

    private Canvas OptionCanv;
    private Canvas soundCanvas;
    private Canvas displayCanvas;

    private CanvasGroup optionCanvasGroup;

    void Start()
    {
        OptionCanvas = GameObject.Find("OptionCanv");
        SoundOptionCanvas = GameObject.Find("SoundCanvas");

        OptionCanv = OptionCanvas.GetComponent<Canvas>();
        soundCanvas = SoundOptionCanvas.GetComponent<Canvas>();

        optionCanvasGroup = OptionCanv.GetComponent<CanvasGroup>();
        optionCanvasGroup.interactable = false;  // 操作不能にする
        optionCanvasGroup.blocksRaycasts = false; // クリックなどのイベントを受け付けなくする
    }

    /**
     * @brief コンテニューボタン用
     * @memo インスペクターで引数にセーブされたシーンを入れてください。
     */
    public void ContinueButton(string _str)  //Startボタン用関数
    {
        irisObject = GameObject.Find("IrisCanv");
        if (!this.irisObject)
        {
            Debug.LogError("IrisCanvが見つからず、取得できませんでした。");
            return;
        }
        UIIrisScript iris = irisObject.GetComponent<UIIrisScript>();
        if (!iris)
        {
            Debug.LogError("UIIrisScriptが見つからず、取得できませんでした。");
            return;
        }
        iris.IrisOut(_str); //次のシーンを代入
    }

    /**
     * @brief ステージセレクトボタン用
     */
    public void SerectButton() //Serectボタン用関数
    {
        SceneManager.LoadScene("StageSelect");//引数にステージセレクトシーンを代入
    }

    /**
     * @brief オプションボタン用
     */
    public void OptionButton()
    {
        OptionCanv.enabled = true;  //OptionCanvasを表示
        soundCanvas.enabled = true;

        optionCanvasGroup.interactable = true;  // 操作可能にする
        optionCanvasGroup.blocksRaycasts = true; // クリックなどのイベントを受け付ける
    }

    /**
     * @brief ゲーム終了関数
     */
    public void GameEndButton() //ゲーム終了
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
        #else
            Application.Quit();//ゲームプレイ終了
        #endif
    }
}
