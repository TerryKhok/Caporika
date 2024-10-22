using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectionEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject shadow;      // 影オブジェクト
    public RectTransform frame;    // ボタン枠

    public Vector2 selectedSize = new Vector2(579, 211);  // 選択時のサイズ
    public Vector2 unselectedSize = new Vector2(579, 211); // 未選択時のサイズ
    public Vector2 selectedPositionOffset = new Vector2(-11, 11); // 選択時の位置オフセット

    private Vector2 originalFramePosition;  // ボタン枠の元の位置

    private void Start()
    {
        // ボタンとボタン枠の元の位置を記録
        originalFramePosition = frame.anchoredPosition;

        // 初期状態では影を非表示
        shadow.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // ポインターがボタンに重なったときの処理
        OnSelect();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // ポインターがボタンから離れたときの処理
        OnDeselect();
    }

    private void OnSelect()
    {
        // ボタン選択時の処理
        shadow.SetActive(true); // 影を表示
        frame.sizeDelta = selectedSize;  // ボタン枠のサイズを変更
        frame.anchoredPosition = originalFramePosition + selectedPositionOffset;   // ボタン枠の位置を調整
    }

    private void OnDeselect()
    {
        // ボタン非選択時の処理
        shadow.SetActive(false); // 影を非表示
        frame.sizeDelta = unselectedSize;  // ボタン枠のサイズを元に戻す
        frame.anchoredPosition = originalFramePosition;   // ボタン枠の位置を元に戻す
    }
}
