using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimEventManager : MonoBehaviour
{
    [SerializeField] private GameObject irisLid;
    [SerializeField] private GameObject irisCanv;
    // Start is called before the first frame update

    /**
     * @brief アイリスアウトアニメーションのイベントで使用
     * @memo 蓋絵を表示(自然に暗転させるため)
     */
    public void IrisOutLidEvent()
    {
        irisLid.SetActive(true);
    }

    /**
     * @brief アイリスアウト終了時のシーンチェンジイベント用
     * @memo  IrisScriptから次のシーンの名前を取ってきている
     */
    public void IrisOutSceneChangeEvent()
    {
        UIIrisScript iris =  this.GetComponent<UIIrisScript>(); 
        SceneManager.LoadScene(iris.nextScene); //引数にステージセレクトシーンを代入
    }

    /**
     * @brief アイリスインアニメーション中のイベント用
     * @memo 穴をふさいでる蓋絵を非表示に
     */
    public void IrisInEvent()
    {
        irisLid.SetActive(false);
    }

    /**
     * @brief アイリスインアニメーション中のイベント用
     * @memo アイリス用キャンバスを非表示に(しないとボタンが押せないため)
     */
    public void IrisInCanvasEvent()
    {
        irisCanv.SetActive(false);
    }
}
