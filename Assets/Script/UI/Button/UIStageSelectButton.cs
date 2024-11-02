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
    public void StageSerectButton(string _str)
    {
        iris.IrisOut(_str); //次のシーンを代入
    }


    /**
     * @brief   ボタンクリック時に実行する関数
     *          シーンロード時に必要な処理を記述する
     */
    public void OnClickButton()
    {
        GimmickCheckpointParam.ResetCheckpointParams();    // チェックポイントのリセット
        // シーンをロードする処理
    }
}
