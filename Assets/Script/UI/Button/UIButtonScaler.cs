using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


/**
* @brief このスクリプトがアタッチされているボタンにカーソルが触れたときに大きくするスクリプト
* @memo インスペクターでscaleMultiplierを変更したら大きくなる倍率が変わるよ
*       
*/
public class UIButtonScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    private RectTransform rectTransform;
    private bool isFocus;   // カーソルがあっているか

    // ボタンの大きさを調整する倍率
    public float scaleMultiplier = 1.2f;

    // 中心から離れている距離に応じた小さくなりやすさ。
    // 数値が大きくなるとより小さくなりやすい
    public float distanceScaleRatio = 0.06f;

    /**
    * @brief ボタンの元のサイズを保存する
    * @memo 
    */
    void Start()
    {
        // 元のサイズを保存
        originalScale = transform.localScale;
        rectTransform = GetComponent<RectTransform>();
    }

    /**
     * @brief   サイズを計算して反映する
     */
    void Update()
    {
        var distanceFromCenter = Mathf.Abs(rectTransform.position.x);   // 距離を取ってくる
        var distanceScaleMulti = 1.0f - (distanceFromCenter * distanceScaleRatio);  // どれくらい小さくするか決める
        var scale = originalScale * distanceScaleMulti; // 基本サイズを決める

        // カーソルがあってたら距離関係なくデカくなる
        if (isFocus)
        {
            scale = originalScale * scaleMultiplier;
            //scale *= scaleMultiplier; // 比率で変えたいだけならこっちにして
        }

        // サイズを変更する
        transform.localScale = scale;
    }

    /**
    * @brief カーソルがボタンに入った時ボタンを大きくする関数
    * @memo 
    */
    public void OnPointerEnter(PointerEventData eventData)
    {
        // ボタンを大きくする
        isFocus = true;
        //transform.localScale = originalScale * scaleMultiplier;
    }

    /**
    * @brief カーソルがボタンから離れた時ボタンを元の大きさに戻す
    * @memo 
    */
    public void OnPointerExit(PointerEventData eventData)
    {
        isFocus = false;
        // ボタンを元の大きさに戻す
        //transform.localScale = originalScale;
    }
}
