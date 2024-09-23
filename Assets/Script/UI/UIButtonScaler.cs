using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButtonScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;

    // ボタンの大きさを調整する倍率
    public float scaleMultiplier = 1.2f;

    void Start()
    {
        // 元のサイズを保存
        originalScale = transform.localScale;
    }

    // カーソルがボタンに入った時
    public void OnPointerEnter(PointerEventData eventData)
    {
        // ボタンを大きくする
        transform.localScale = originalScale * scaleMultiplier;
    }

    // カーソルがボタンから離れた時
    public void OnPointerExit(PointerEventData eventData)
    {
        // ボタンを元の大きさに戻す
        transform.localScale = originalScale;
    }
}
