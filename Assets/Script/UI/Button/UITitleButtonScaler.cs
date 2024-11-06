using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectionEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform frame;    // ボタン枠
    private RectTransform button;//ボタン

    public Vector2 selectedPositionOffset = new Vector2(-5, 5); // 選択時の位置オフセット
    public Vector2 pressOffset = new Vector2(5, -5); // 選択時の位置オフセット

    private Vector2 originalFramePosition;  // ボタン枠の元の位置
    private Vector2 originalButtonPosition;  // ボタン枠の元の位置

    private void Start()
    {
        button = this.GetComponent<RectTransform>();
        // ボタンとボタン枠の元の位置を記録
        originalFramePosition = frame.anchoredPosition;
        originalButtonPosition = button.anchoredPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // ポインターがボタンに重なったときの処理
        SoundManager.Instance.PlaySE("MENU_MOVE");
        frame.anchoredPosition = originalFramePosition + selectedPositionOffset;   // ボタン枠の位置を調整
        button.anchoredPosition = originalButtonPosition + selectedPositionOffset;   // ボタンの位置を調整
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // ポインターがボタンから離れたときの処理
        frame.anchoredPosition = originalFramePosition;   // ボタン枠の位置を元に戻す
        button.anchoredPosition = originalButtonPosition;   // ボタンの位置を元に戻す
    }

    // ボタンが押されたとき
    public void OnPointerDown(PointerEventData eventData)
    {
        // ボタンをさらに下げ、影と重なるようにする
        SoundManager.Instance.PlaySE("MENU_SELECT");
        frame.localPosition = originalFramePosition + pressOffset;
        button.localPosition = originalButtonPosition + pressOffset;
    }

    // ボタンが押されなくなったとき
    public void OnPointerUp(PointerEventData eventData)
    {
        // ボタンをさらに下げ、影と重なるようにする
        frame.localPosition = originalFramePosition + selectedPositionOffset;
        button.localPosition = originalButtonPosition + selectedPositionOffset;
    }


}
