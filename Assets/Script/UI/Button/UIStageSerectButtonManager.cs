//UIStageSerectButtonManager
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIStageSerectButtonManager : MonoBehaviour
{
    public GameObject buttonContainer;  // ボタンを含む親オブジェクト
    public Button leftArrowButton;
    public Button rightArrowButton;
    public float slideDuration = 0.5f;  // スライドにかかる時間
    public float buttonWidth = 100f;    // 各ボタンの幅
    public int visibleButtonCount = 5;  // 画面上に表示されるボタンの数
    public int currentIndex = 0;       // 現在の左端のボタンのインデックス
    private int totalButtons;           // 全ボタンの数
    private bool isSliding = false;

    void Start()
    {
        totalButtons = buttonContainer.transform.childCount;
        UpdateArrowButtons();
        // 初期位置を設定
        buttonContainer.transform.localPosition = Vector3.zero;
    }

    public void OnRightArrowClick()
    {
        // 右にスライド可能かチェック
        if (currentIndex + 1 < totalButtons && !isSliding)
        {
            StartCoroutine(SlideButtons(-buttonWidth));  // 右へ移動
            currentIndex++;
        }
    }

    public void OnLeftArrowClick()
    {
        // 左にスライド可能かチェック
        if (currentIndex > 0 && !isSliding)
        {
            StartCoroutine(SlideButtons(buttonWidth));  // 左へ移動
            currentIndex--;
        }
    }

    private IEnumerator SlideButtons(float distance)
    {
        isSliding = true;
        float elapsed = 0f;
        Vector3 startPosition = buttonContainer.transform.localPosition;
        Vector3 targetPosition = startPosition + new Vector3(distance, 0, 0);

        while (elapsed < slideDuration)
        {
            buttonContainer.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsed / slideDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        buttonContainer.transform.localPosition = targetPosition;
        isSliding = false;
        UpdateArrowButtons();
    }

    private void UpdateArrowButtons()
    {
        // 左右の矢印ボタンの有効/無効を更新
        leftArrowButton.interactable = currentIndex > 0;
        rightArrowButton.interactable = (currentIndex + 1 < totalButtons);
    }
}

