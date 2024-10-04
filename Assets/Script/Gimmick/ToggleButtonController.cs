using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/**
 * @brief   トグルボタンの制御スクリプト
 * 
 * @memo    ・OnCollisionEnter2Dでオンオフを切り替える
 *          ・Onになったときの関数ButtonEnable()
 *          ・Offになった時の関数ButtonDisable()
 */
public class ToggleButtonController : MonoBehaviour
{

    public float buttonMass;        // ボタンの重さ
    private bool touched = false;   // 触った瞬間であることを示すフラグ
    private bool isOn = false;      // ボタンの状態を表す変数（オンならtrue、オフならfalse）

    // UnityEvent型のイベントを管理する変数
    public UnityEvent onEvent;
    public UnityEvent offEvent;

    public GimmickEventTrigger eventTrigger;
    public List<GameObject> touchedObjects = new List<GameObject>();   // ボタンを触っているオブジェクトのリスト
    private Transform parentTransform;              // 変形用の親のTransform
    private Vector3 originScale = Vector3.zero;    // 元のボタンの大きさ


    private void Start()
    {
        // 親のTransformを取得して、元のサイズを記録
        this.parentTransform = transform.parent;
        this.originScale = this.parentTransform.localScale;
    }

    private void FixedUpdate()
    {
        if (touched)
        {
            // トグルの状態に応じてイベントを発火
            if (this.isOn)
            {
                ButtonEnable();
            }
            else
            {
                ButtonDisable();
            }
        }
    }

    // 2Dコライダーが衝突した際に呼び出される関数
    public void Triggered()
    {
        Collider2D collider = this.eventTrigger.GetTriggeredCollider();
        if (collider.CompareTag("Player") || collider.CompareTag("Enemy"))
        {
            this.touchedObjects.Add(collider.gameObject);
            // 直前に処理が走ってなくてかつ重さが十分なら
            if (CheckedMass())
            {
                this.isOn = !this.isOn;
                // トグルの状態に応じてイベントを発火
                if (this.isOn)
                {
                    ButtonEnable();
                }
                else
                {
                    ButtonDisable();
                }
            }
        }
    }

    public void TriggerExit()
    {
        Collider2D collider = this.eventTrigger.GetTriggeredCollider();
        this.touchedObjects.Remove(collider.gameObject);
    }

    private void ButtonEnable()
    {
        Debug.Log("ToggleButtonEnabled");
        this.onEvent.Invoke();

        // ボタンを狭めて押した感を出す
        //this.touched = false;   // 処理が終わったらフラグを折る
        float newScaleY = 0.4f;
        this.parentTransform.localScale = new Vector3(this.parentTransform.localScale.x, newScaleY, this.parentTransform.localScale.z);
    }

    private void ButtonDisable()
    {
        Debug.Log("ToggleButtonDisabled");
        this.offEvent.Invoke();

        // ボタンを伸ばして元に戻す
        //this.touched = false;   // 処理が終わったらフラグを折る
        float newScaleY = this.originScale.y;
        this.parentTransform.localScale = new Vector3(this.parentTransform.localScale.x, newScaleY, this.parentTransform.localScale.z);
    }

    // 重さを判定する処理
    private bool CheckedMass()
    {
        // 触っているオブジェクトのサイズの合計を計算
        int totalSize = 0;
        foreach (GameObject obj in this.touchedObjects)
        {
            CharaState charaState = obj.GetComponent<CharaState>();
            if (charaState != null)
            {
                totalSize += charaState.GetCharaWeight();
            }
        }

        // ボタンよりも重いとき押した
        if (totalSize >= this.buttonMass) { return true; }
        return false;
    }
}
