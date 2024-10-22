using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStageSelectButton : MonoBehaviour
{
    [SerializeField] private string sceneName;  // ボタンクリック時に遷移するシーン名

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
