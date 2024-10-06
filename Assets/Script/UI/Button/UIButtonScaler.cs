using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


/**
* @brief このスクリプトがアタッチされているボタンにカーソルが触れたときに大きくするスクリプト
* @memo インスペクターでscaleMultiplierを変更したら大きくなる倍率が変わるよ
*/   
public class UIButtonScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;

    // ボタンの大きさを調整する倍率
    public float scaleMultiplier = 1.2f;

/**
* @brief ボタンの元のサイズを保存する
* @memo 
*/   
    void Start()
    {
        // 元のサイズを保存
        originalScale = transform.localScale;
    }

/**
* @brief カーソルがボタンに入った時ボタンを大きくする関数
* @memo 
*/   
    public void OnPointerEnter(PointerEventData eventData)
    {
        // ボタンを大きくする
        transform.localScale = originalScale * scaleMultiplier;
    }

/**
* @brief カーソルがボタンから離れた時ボタンを元の大きさに戻す
* @memo 
*/   
    public void OnPointerExit(PointerEventData eventData)
    {
        // ボタンを元の大きさに戻す
        transform.localScale = originalScale;
    }
}
