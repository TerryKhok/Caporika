using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomSlider : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    public RectTransform sliderBackground; // スライダー背景 (SliderBaseの子オブジェクト)
    public RectTransform unfilledImage;    // 埋められていない部分 (SliderBaseの子オブジェクト)
    public RectTransform handle;           // ハンドル (SliderBaseの子オブジェクト)

    private float sliderWidth;
    private float minHandleX; // ハンドルの可動域の最小値
    private float maxHandleX; // ハンドルの可動域の最大値
    private Vector2 backgroundPosition;  // 背景画像の座標

    [Header("Slider Values")]
    [Range(0, 1)] public float value = 0;  // スライダーの値 (0~1)

    private bool isDragging = false; // ハンドルがドラッグ中かどうかを判定するフラグ

    void Start()
    {
        // 背景画像の幅と位置を取得
        sliderWidth = sliderBackground.rect.width;
        backgroundPosition = sliderBackground.anchoredPosition;

        // 背景画像のX座標に基づいて、ハンドルの可動範囲を計算
        minHandleX = backgroundPosition.x - (sliderWidth / 2);
        maxHandleX = backgroundPosition.x + (sliderWidth / 2);

        // スライダーの初期状態を反映
        UpdateSlider();
    }

    void Update()
    {
        // ドラッグしていない間はValueに従ってハンドルとunfilledImageを更新
        if (!isDragging)
        {
            UpdateSlider();
        }
    }

    // スライダーの更新 (Valueに基づいてハンドルとunfilledImageを更新)
    private void UpdateSlider()
    {
        // ハンドルの位置を更新 (minHandleXが左端、maxHandleXが右端)
        float handlePosX = Mathf.Lerp(minHandleX, maxHandleX, value);
        handle.anchoredPosition = new Vector2(handlePosX, handle.anchoredPosition.y);

        // "埋められていない部分"の画像を更新
        float unfilledWidth = maxHandleX - handlePosX; // ハンドル位置から右端までの幅を計算

        // unfilledImageの中心座標を、ハンドルの位置と背景画像の右端の中間に設定
        float unfilledPosX = (handlePosX + maxHandleX) / 2;

        // unfilledImageのサイズと位置を更新
        unfilledImage.sizeDelta = new Vector2(unfilledWidth, unfilledImage.sizeDelta.y);
        unfilledImage.anchoredPosition = new Vector2(unfilledPosX, unfilledImage.anchoredPosition.y);
    }

    // ドラッグ時にハンドルを動かす
    public void OnDrag(PointerEventData eventData)
    {
        isDragging = true;
        UpdateHandlePosition(eventData);
    }

    // クリックした位置にハンドルを移動
    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        UpdateHandlePosition(eventData);
    }

    // ドラッグ終了時にフラグをリセット
    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }

    // ハンドル位置を更新するヘルパー
    private void UpdateHandlePosition(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(sliderBackground, eventData.position, eventData.pressEventCamera, out localPoint);

        // 背景画像内のローカル座標に基づいて0~1に正規化
        float newValue = Mathf.Clamp01((localPoint.x - minHandleX) / (maxHandleX - minHandleX));

        // 新しい値をValueに代入し、同期させる
        value = newValue;

        // インスペクターの値を強制的に更新
        #if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
        #endif

        UpdateSlider();
    }

    // Inspectorで値が変更された場合の同期
    private void OnValidate()
    {
        UpdateSlider();
    }
}
