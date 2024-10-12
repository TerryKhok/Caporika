using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIResultButton : MonoBehaviour
{

    private GameObject irisObject;
    public void Start()
    {
        irisObject = GameObject.Find("IrisCanv");
        if (!this.irisObject)
        {
            Debug.LogError("IrisCanvが見つからず、取得できませんでした。");
            return;
        }
    }

    /**
     * @brief タイトルシーンに行く関数
     * @memo アイリスアウトを呼び出し、引数にタイトルシーンを代入
     */  
    public void TitleButton()
    {
        UIIrisScript iris = irisObject.GetComponent<UIIrisScript>();
        iris.IrisOut("TitleScene"); //次のシーンを代入

    }

    /**
     * @brief 次のシーンに行く関数
     * @memo アイリスアウトを呼び出し、引数に次のシーンを代入
     */  
        public void NextButton()
    {
        UIIrisScript iris = irisObject.GetComponent<UIIrisScript>();
        iris.IrisOut(""); //次のシーンを代入

    }
}
