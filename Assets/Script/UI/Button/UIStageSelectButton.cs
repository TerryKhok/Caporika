using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStageSelectButton : MonoBehaviour
{
    [SerializeField] private string sceneName;  // ボタンクリック時に遷移するシーン名
    private GameObject irisObject;
    private UIIrisScript iris;

    /**
    * @brief 初期化
    * @memo 
    */
    void Start()
    {
        irisObject = GameObject.Find("IrisCanv");
        iris = irisObject.GetComponent<UIIrisScript>();
    }


    /**
    * @brief ステージセレクトボタン用
    * @memo 引数にシーン名を代入
    */
    public void StageSelectButton(string _str)
    {
        SoundManager.Instance.PlaySE("MENU_SELECT");
        GimmickCheckpointParam.ResetCheckpointParams();    // チェックポイントのリセット
        iris.IrisOut(_str); //次のシーンを代入
    }
}
