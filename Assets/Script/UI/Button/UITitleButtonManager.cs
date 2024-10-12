using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonManager : MonoBehaviour
{
     [SerializeField] private GameObject irisObject;

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
