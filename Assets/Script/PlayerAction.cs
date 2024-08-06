using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

/**
 *  @brief 	キャラの行動をまとめた
 *  
 *  @memo   ・飛び出る処理
 *          ・中に入る処理
 *          ・攻撃(未実装)
*/
public class PlayerAction : MonoBehaviour
{
    public float jumpForce = 10.0f;                     // 力の大きさ
    public string tagName;                              // マトリョーシカの親タグ名

    private bool isSametag = false;                     // 同じTag名
    private Collider2D currentTrigger = null;           // 今当たっているオブジェクト
    private CharaState triggerState = null;             // 当たっているオブジェクトの状態

    private CharaState matryoishkaState = null;         // 今の状態
    private MatryoshkaManager matryoshkaManager = null;
    private int sizeState = 0;                          // 自身のサイズ

    private void Start()
    {
        matryoishkaState = GetComponent<CharaState>();              // このマトリョーシカの状態
        sizeState = matryoishkaState.GetCharaSize();                // このマトリョーシカの大きさ
        matryoshkaManager = FindAnyObjectByType<MatryoshkaManager>();   
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 自分が一番小さくない
            if (sizeState > 1)
            {
                // 飛び出す処理
                PopOut();
                // このマトリョーシカの状態を「死んだ」に
                matryoishkaState.SetCharaState(CharaState.State.Dead);
            }

        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // タグ名が同じで、自分よりも1個大きいマトリョーシカのとき
            if (isSametag&& sizeState + 1 == triggerState.GetCharaSize())
            {
                // 入る処理
                NestInside(); 
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // タグ名が同じとき、当たったオブジェクトを保持
        if (other.CompareTag(tagName))
        {
            isSametag = true;
            currentTrigger = other;
            triggerState = other.gameObject.GetComponentInParent<CharaState>(); // マトリョーシカの状態
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // 同じタグから出たとき、リセット
        if (other.CompareTag(tagName))
        {
            isSametag = false;
            currentTrigger = null;
            triggerState = null;
        }
    }

    /**
     * @brief 	マトリョーシカの中に入る処理
    */
    void NestInside()
    {
        // 親のマトリョーシカのスクリプトを取得
        PlayerMove triggerMove = triggerState.GetComponentInParent<PlayerMove>();
        PlayerAction triggerAction = triggerState.GetComponentInParent<PlayerAction>();

        // 入るマトリョーシカにアタッチされているスクリプトを全て有効に
        if (triggerMove != null) triggerMove.enabled = true;
        if (triggerAction != null) triggerAction.enabled = true;

        // 入るマトリョーシカの状態を「通常」にする
        triggerState.GetComponentInParent<CharaState>().SetCharaState(CharaState.State.Normal);

        // このオブジェクトを消す
        Destroy(gameObject);
    }

    /**
     * @brief 	マトリョーシカから飛び出る処理
    */
    void PopOut()
    {
        // 現在のマトリョーシカの位置、角度を取得
        Vector2 position = transform.position;
        Quaternion rotation = transform.rotation;

        // 自身より一段階小さいマトリョーシカを生成
        GameObject newobj = matryoshkaManager.InstanceMatryoshka(sizeState - 1);
        if (newobj == null)
        {
            Debug.LogError("マトリョーシカの生成に失敗");
            return;
        }

        // 外側のマトリョーシカと同じ回転、座標にセット
        newobj.transform.position = position;
        newobj.transform.rotation = rotation;

        // 生成したマトリョーシカのRigidbody2Dを取得
        Rigidbody2D rb = newobj.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = newobj.AddComponent<Rigidbody2D>();
            rb.isKinematic = false; // 物理演算を有効化
        }

        // 角度をラジアンに変換
        float eulerAngleZ = rotation.eulerAngles.z;
        float angleInRadians = eulerAngleZ * Mathf.Deg2Rad;
        Vector2 forceDirection = Vector2.zero;

        // 傾きがあるときの計算
        if (angleInRadians != 0)
        {
            // 角度に基づくベクトルの調整
            float angle = 0.0f;
            if (eulerAngleZ >= 0 && eulerAngleZ <= 90)
            {
                // 左に傾いている場合
                angle = 90 - eulerAngleZ;
            }
            else if (eulerAngleZ >= 270 && eulerAngleZ <= 360)
            {
                // 右に傾いている場合
                angle = eulerAngleZ - 270;
            }

            // 斜め方向のベクトルを計算
            forceDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            // ベクトルを正規化して調節
            forceDirection.Normalize();
            forceDirection.x *= 0.5f;
            forceDirection.y *= 0.5f;

            // 角度に基づくベクトルの調整
            if (eulerAngleZ >= 0 && eulerAngleZ <= 90)
            {
                // 左に傾いている場合
                forceDirection.x = -Mathf.Abs(forceDirection.x); // x軸をマイナス
                forceDirection.y = Mathf.Abs(forceDirection.y);  // y軸をプラス
            }
            else if (eulerAngleZ >= 270 && eulerAngleZ <= 360)
            {
                // 右に傾いている場合
                forceDirection.x = Mathf.Abs(forceDirection.x);  // x軸をプラス
                forceDirection.y = Mathf.Abs(forceDirection.y);  // y軸をプラス
            }
        }    
        else
        {
            // 0度の時はまっすぐ飛ばす
            forceDirection = Vector2.up * 0.5f;
        }

        //// 力の方向
        //Debug.Log("Force Direction After Adjustment: " + forceDirection);
        //Debug.Log("Velocity before AddForce: " + rb.velocity);

        // 力を加える
        rb.AddForce(forceDirection * jumpForce, ForceMode2D.Impulse);
        //Debug.Log("Velocity after AddForce: " + rb.velocity);

        // 飛び出たマトリョーシカを「飛んだ」状態に
        CharaState newState = newobj.GetComponent<CharaState>();
        if (newState != null)
        {
            newState.SetCharaState(CharaState.State.Flying);
        }
        else{ Debug.LogError("newStateがnull"); }

        // 移動スクリプトを無効化
        PlayerMove moveScript = GetComponent<PlayerMove>();
        if (moveScript != null)
        {
            // 動きを止める
            Rigidbody2D moveRb = moveScript.GetComponent<Rigidbody2D>();
            if (moveRb != null)
            {
                moveRb.velocity = Vector2.zero;
            }
        }

        // このスクリプトを無効化
        enabled = false;
    }
}