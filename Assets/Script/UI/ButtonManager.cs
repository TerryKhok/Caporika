using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    //二回目以降のプレイ
    public void ContinueButton()  //Startボタン用関数
    {
        //SceneManager.LoadScene();//引数にセーブされたステージ
    }
    public void SerectButton() //Serectボタン用関数
    {
        //SceneManager.LoadScene();//引数にステージセレクトシーンを代入
    }
    public void OptionButton() //Optionボタン用関数
    {
        //SceneManager.LoadScene();//OptionをSceneにする場合のみ使用
    }
    public void GameEndButton() //ゲーム終了
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
    }
}
