using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;  // イベント用の名前空間を追加

public class CustomSlider : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    public RectTransform sliderBackground;
    public RectTransform unfilledImage;
    public RectTransform handle;

    private float sliderWidth;
    private float minHandleX;
    private float maxHandleX;
    private Vector2 backgroundPosition;

    [Header("Slider Values")]
    [Range(0, 1)] public float value = 0;

    private bool isDragging = false;

    public UnityEvent<float> onValueChanged;  // スライダー値変更時のイベント

    void Start()
    {
        sliderWidth = sliderBackground.rect.width;
        backgroundPosition = sliderBackground.anchoredPosition;
        minHandleX = backgroundPosition.x - (sliderWidth / 2);
        maxHandleX = backgroundPosition.x + (sliderWidth / 2);
        UpdateSlider();
    }

    void Update()
    {
        if (!isDragging)
        {
            UpdateSlider();
        }
    }

    private void UpdateSlider()
    {
        float handlePosX = Mathf.Lerp(minHandleX, maxHandleX, value);
        handle.anchoredPosition = new Vector2(handlePosX, handle.anchoredPosition.y);

        float unfilledWidth = maxHandleX - handlePosX;
        float unfilledPosX = (handlePosX + maxHandleX) / 2;

        unfilledImage.sizeDelta = new Vector2(unfilledWidth, unfilledImage.sizeDelta.y);
        unfilledImage.anchoredPosition = new Vector2(unfilledPosX, unfilledImage.anchoredPosition.y);
    }

    public void OnDrag(PointerEventData eventData)
    {
        isDragging = true;
        UpdateHandlePosition(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        UpdateHandlePosition(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }

    private void UpdateHandlePosition(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(sliderBackground, eventData.position, eventData.pressEventCamera, out localPoint);

        // 背景画像内のローカル座標に基づいて0~1に正規化
        float newValue = Mathf.Clamp01((localPoint.x - minHandleX) / (maxHandleX - minHandleX));

        // 新しい値をValueに代入し、同期させる
        value = newValue;

        // 値が正しく設定されているか確認
        Debug.Log("CustomSlider Value: " + value);

        // onValueChanged イベントが正しく発火しているか確認
        onValueChanged?.Invoke(value);

        // インスペクターの値を強制的に更新
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif

        UpdateSlider();
    }


    private void OnValidate()
    {
        UpdateSlider();
    }
}
