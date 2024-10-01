using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimSplashScreenEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void SplashScreenAnimEvent()
    {
        SceneManager.LoadScene("TitleScene");//引数にステージタイトルシーンを代入
    }
}
