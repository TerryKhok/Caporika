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
    private Transform parentTransform;  // 押したときの見た目の処理用
    private Vector3 origineScale = Vector3.zero;                        // 元のボタンの大きさ


    // Start is called before the first frame update
    void Start()
    {
        // 元の大きさを保持
        parentTransform = this.transform.parent;
        origineScale = parentTransform.localScale;
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
                if (!isTriggerEventRelease)
                {
                    // ボタンが離されたときのイベントを呼び出す
                    onReleased.Invoke();
                    isTriggerEventRelease = true;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ボタンを触ってきたオブジェクトをリストに追加
        isTouched = true;
        touchedObjects.Add(collision.gameObject);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // ボタンを触っていたオブジェクトをリストから削除
        touchedObjects.Remove(collision.gameObject);
        if (touchedObjects.Count <= 0)
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
        float newScaleY = parentTransform.localScale.y - pressSpeed;
        if (newScaleY < 0.2f)
        {
            newScaleY = 0.2f;
            isPressedButton = true;
        }
        parentTransform.localScale = new Vector3(parentTransform.localScale.x, newScaleY, parentTransform.localScale.z);
        return isPressedButton;
    }

    // ボタンを離す処理
    private bool ReleasedButton()
    {
        bool isReleasedButton = false;

        // ボタンを伸ばして元に戻す
        float newScaleY = parentTransform.localScale.y + pressSpeed;
        if (newScaleY > origineScale.y)
        {
            newScaleY = origineScale.y;
            isReleasedButton = true;
        }
        parentTransform.localScale = new Vector3(parentTransform.localScale.x, newScaleY, parentTransform.localScale.z);
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
                totalSize += charaState.GetCharaWeight();
            }
        }

        // ボタンよりも重いとき押した
        if (totalSize >= buttonMass) { return true; }
        return false;
    }
}