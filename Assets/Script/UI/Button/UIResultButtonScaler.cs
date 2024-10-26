using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIResultButtonScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform button;//ボタン

    public Vector2 selectedPositionOffset = new Vector2(5, -5); // 選択時の位置オフセット
    public Vector2 pressOffset = new Vector2(10, -10); // 選択時の位置オフセット
    private Vector2 originalButtonPosition;  // ボタン枠の元の位置
    // Start is called before the first frame update
    void Start()
    {
        button = this.GetComponent<RectTransform>();
        // ボタンの元の位置を記録
        originalButtonPosition = button.anchoredPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // ポインターがボタンに重なったときの処理
        button.anchoredPosition = originalButtonPosition + selectedPositionOffset;   // ボタンの位置を調整
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // ポインターがボタンから離れたときの処理
        button.anchoredPosition = originalButtonPosition;   // ボタンの位置を元に戻す
    }

    // ボタンが押されたとき
    public void OnPointerDown(PointerEventData eventData)
    {
        // ボタンをさらに下げ、影と重なるようにする
        button.localPosition = originalButtonPosition + pressOffset;
    }

    // ボタンが押されなくなったとき
    public void OnPointerUp(PointerEventData eventData)
    {
        // ボタンをさらに下げ、影と重なるようにする
        button.localPosition = originalButtonPosition + selectedPositionOffset;
    }
}
