using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    
    public void StartButtonFunciton()  //Startボタン用関数
    {
        //SceneManager.LoadScene();//引数にセーブされたステージ、あるいは最初のステージを代入
    }

    public void SerectButtonFunciton() //Serectボタン用関数
    {
        //SceneManager.LoadScene();//引数にステージセレクトシーンを代入
    }
    public void OptionButtonFunciton() //Optionボタン用関数
    {
        //SceneManager.LoadScene();//OptionをSceneにする場合のみ使用
    }
    public void GameEndButtonFunciton() //ゲーム終了
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
    }
}
