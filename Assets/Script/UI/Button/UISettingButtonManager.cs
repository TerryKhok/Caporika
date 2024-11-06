using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;



/**
* @brief 設定変更画面で使うスクリプト
* @memo 設定のジャンルをボタンを押して変更するときに使う
*/
public class UISettingButtonManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private GameObject SoundCanvObj;    //SoundButtonをインスペクターで入れる
    [SerializeField] private GameObject DisplayCanvObj;  //DisplayButtonをインスペクターで入れる

    public GameObject ButtonFlame;
    public RectTransform flameTransform;    // ボタン枠
    public RectTransform buttonTransform;       //ボタン

    private Canvas SoundCanvas;
    private Canvas DisplayCanvas;

    public Vector2 selectedPositionOffset = new Vector2(11, -11); // 選択時の位置オフセット
    public Vector2 pressOffset = new Vector2(11, -11);  //押されたときの位置
    private Vector2 originalButtonPosition;  // ボタン枠の元の位置
    private Vector2 originalFramePosition;  // ボタン枠の元の位置



    /**
    * @brief 初期化
    * @memo 
*/
    public void Start()
    {
        SoundCanvas = SoundCanvObj.GetComponent<Canvas>();
        DisplayCanvas = DisplayCanvObj.GetComponent<Canvas>();

        flameTransform = ButtonFlame.GetComponent<RectTransform>();
        buttonTransform = this.GetComponent<RectTransform>();

        // ボタンとボタン枠の元の位置を記録
        originalFramePosition = flameTransform.anchoredPosition;
        originalButtonPosition = buttonTransform.anchoredPosition;
    }

    /**
    * @brief Soundのボタンが押されたときに他二つを非表示にする
    * @memo 
*/
    public void SoundButtonEvent()
    {
        SoundCanvas.enabled = true;
        DisplayCanvas.enabled = false;
    }

    /**
    * @brief Displayのボタンが押されたときに他二つを非表示にする
    * @memo 
*/
    public void DisplayButtonEvent()
    {
        SoundCanvas.enabled = false;
        DisplayCanvas.enabled = true;
    }


    /**
     * @brief カーソルがボタンに入った時白縁を表示する
     * @memo 
     */
    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySE("MENU_MOVE");
        ButtonFlame.SetActive(true);
        flameTransform.anchoredPosition = originalFramePosition + selectedPositionOffset;   // ボタン枠の位置を調整
        buttonTransform.anchoredPosition = originalButtonPosition + selectedPositionOffset;   // ボタンの位置を調整
    }

    // ボタンが押されたとき
    public void OnPointerDown(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySE("MENU_SELECT");
        // ボタンをさらに下げ、影と重なるようにする
        flameTransform.localPosition = originalFramePosition + pressOffset;
        buttonTransform.localPosition = originalButtonPosition + pressOffset;
    }

    // ボタンが押されなくなったとき
    public void OnPointerUp(PointerEventData eventData)
    {
        // ボタンをさらに下げ、影と重なるようにする
        flameTransform.localPosition = originalFramePosition + selectedPositionOffset;
        buttonTransform.localPosition = originalButtonPosition + selectedPositionOffset;
    }

    /**
    * @brief カーソルがボタンから離れた時白縁を非表示にする
    * @memo 
    */
    public void OnPointerExit(PointerEventData eventData)
    {
        ButtonFlame.SetActive(false);
        flameTransform.anchoredPosition = originalFramePosition;   // ボタン枠の位置を元に戻す
        buttonTransform.anchoredPosition = originalButtonPosition;   // ボタン枠の位置を元に戻す
    }

}
