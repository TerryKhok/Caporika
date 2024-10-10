using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIResultButton : MonoBehaviour
{

    private GameObject irisObject;
    public void Start()
    {
        irisObject = GameObject.Find("IrisCanv");
    }
    public void TitleButton()
    {
        UIIrisScript iris = irisObject.GetComponent<UIIrisScript>();
        iris.IrisOut("TitleScene"); //次のシーンを代入

    }
}
