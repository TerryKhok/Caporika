using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIOptionButtonScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{

    private RectTransform button;//ボタン
    public GameObject shadowImage;
    public Vector2 selectedPositionOffset = new Vector2(-11, 11); // 選択時の位置オフセット
    private Vector2 originalButtonPosition;  // ボタン枠の元の位置
    // Start is called before the first frame update
    void Start()
    {
        button = this.GetComponent<RectTransform>();
        // ボタンの元の位置を記録
        originalButtonPosition = button.anchoredPosition;
        shadowImage.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // ポインターがボタンに重なったときの処理
        button.anchoredPosition = originalButtonPosition + selectedPositionOffset;   // ボタンの位置を調整
        shadowImage.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // ポインターがボタンから離れたときの処理
        button.anchoredPosition = originalButtonPosition;   // ボタンの位置を元に戻す
        shadowImage.SetActive(false);
    }

    // ボタンが押されたとき
    public void OnPointerDown(PointerEventData eventData)
    {
        // ボタンをさらに下げ、影と重なるようにする
        button.localPosition = originalButtonPosition;
    }

    // ボタンが押されなくなったとき
    public void OnPointerUp(PointerEventData eventData)
    {
        // ボタンをさらに下げ、影と重なるようにする
        button.localPosition = originalButtonPosition + selectedPositionOffset;
    }
}
