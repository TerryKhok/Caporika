using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameButton : MonoBehaviour
{
    public GameObject OptionCanvas;

    public void ContinueButton()
    {
        SystemPose poseCanvas= GameObject.Find ("PoseCanvas").GetComponent<SystemPose>();
        poseCanvas.PoseEnd();
    }

    public void OptionButton()
    {
        
    }

    public void RestartButton()
    {
        //SceneManager.LoadScene("今のシーン");//引数に現在のシーンを代入

    }

    public void TitleButton()
    {
        SceneManager.LoadScene("TitleScene");//引数にステージタイトルシーンを代入

    }

    public void StageSelectButton()
    {
        SceneManager.LoadScene("StageSelect");//引数にステージセレクトシーンを代入
    }
}
