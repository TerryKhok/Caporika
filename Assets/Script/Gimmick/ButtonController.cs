using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/**
 *  @brief 	ボタンの動き
*/
public class ButtonController : MonoBehaviour
{
    public float pressSpeed;        // ボタンが押し込まれる速さ
    public float buttonMass;        // ボタンの重さ
    public UnityEvent onPressed;    // ボタンが押されたときのイベント
    public UnityEvent onReleased;   // ボタンが離されたときのイベント
    private bool isTriggerEventPress = false;
    private bool isTriggerEventRelease = false;


    private bool isTouched = false;                 // true:ボタンに触った
    private bool isPressed = false;                 // true:ボタンを押した
    private bool isReleased = false;                // true:ボタンを離した

    private List<GameObject> touchedObjects = new List<GameObject>();   // ボタンを触っているオブジェクトのリスト
    private Vector3 origineScale = Vector3.zero;                        // 元のボタンの大きさ

    // Start is called before the first frame update
    void Start()
    {
        // 元の大きさを保持
        origineScale = transform.localScale;
    }

    private void FixedUpdate()
    {
        // 触られたとき
        if (isTouched && touchedObjects.Count > 0)
        { 
            // 押されていなければ
            if (!isPressed)
            {
                // ボタンを下げる処理
                isPressed = PressedButton();
            }
            // 押された時
            else
            {        
                // 重さを判定
                if (!CheckedMass() && touchedObjects.Count > 0)
                { 
                    // ボタンのほうが重かったらボタンを押し上げる
                    ReleasedButton();
                }
                else
                {
                    if (!isTriggerEventPress)
                    {
                        // ボタンが押されたときのイベントを呼び出す
                        onPressed.Invoke();
                        isTriggerEventPress = true;
                    }
                }
            }
        }
        // ボタンが戻っていなければ戻す
        else if (!isReleased)
        {
            if (ReleasedButton())
            {
                if(!isTriggerEventRelease)
                {
                    // ボタンが離されたときのイベントを呼び出す
                    onReleased.Invoke();
                    isTriggerEventRelease=true;
                }
                isReleased = true;
            }
        }
        // 離された時
        else if (isReleased)
        {
            // イベント状態をリセット
            isTriggerEventPress = false;
            isTriggerEventRelease = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ボタンを触ってきたオブジェクトをリストに追加
        isTouched = true;
        touchedObjects.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // ボタンを触っていたオブジェクトをリストから削除
        touchedObjects.Remove(collision.gameObject);
        if(touchedObjects.Count <= 0)
        {
            isTouched = false;
        }
        isPressed = false;
        isReleased = false;
    }

    // ボタンを押す処理
    private bool PressedButton()
    {
        bool isPressedButton = false;

        // ボタンを狭めて押した感を出す
        float newScaleY = transform.localScale.y - pressSpeed;
        if (newScaleY < 0.3f)
        {
            newScaleY = 0.3f;
            isPressedButton = true;
        }
        transform.localScale = new Vector3(transform.localScale.x, newScaleY, transform.localScale.z);
        return isPressedButton;
    }

    // ボタンを離す処理
    private bool ReleasedButton()
    {
        bool isReleasedButton = false;

        // ボタンを伸ばして元に戻す
        float newScaleY = transform.localScale.y + pressSpeed;
        if (newScaleY > origineScale.y) 
        { 
            newScaleY = origineScale.y;
            isReleasedButton = true;
        }
        transform.localScale = new Vector3(transform.localScale.x, newScaleY, transform.localScale.z);
        return isReleasedButton;
    }

    // 重さを判定する処理
    private bool CheckedMass()
    {
        // 触っているオブジェクトのサイズの合計を計算
        int totalSize = 0;
        foreach (GameObject obj in touchedObjects)
        {
            CharaState charaState = obj.GetComponent<CharaState>();
            if (charaState != null)
            {
                totalSize += charaState.GetCharaSize();
            }
        }

        // ボタンよりも重いとき押した
        if (totalSize >= buttonMass) { return true; }
        return false;
    }
}