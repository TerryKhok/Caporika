using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

/**
 *  @brief 	プレイヤーの移動
 *  
 *  @memo   ・移動方向と反対の向きに傾く
 *          ・止まった時指定回数揺れて止まる
 *          ・飛んでいるときはゆっくり移動できる
 *          
 *          止まっているときはisKinematicを無効にしている
*/
public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5.0f;          // 移動速度
    public float slowSpeed = 1.0f;          // 飛んでいるときの速度
    public float tiltAmount = 45.0f;        // 回転角の最大値
    public float returnSpeed = 2.0f;        // 回転を元に戻す速度
    public float damping = 0.1f;            // 減衰係数
    public float inputDeadZone = 0.1f;      // 入力のデッドゾーン
    public int maxSwimg = 3;                // 何回揺れるか 
    public float centerOfMassOffset = 0.6f; // 重心の位置の割合

    private Rigidbody2D rb;
    private float tiltVelocity = 0f;        // 傾きの速度
    private int swingCount = 0;             // 揺れの回数をカウント
    private bool isInDeadZone = false;      // デッドゾーン内にいるかどうか
    private float angleSwingZone = 1.0f;    // 揺れた判定内かどうか

    CharaState matryoshkaState = null;      // マトリョーシカの状態

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 重心を下に設定
        if (rb != null)
        {
            Renderer renderer = GetComponent<Renderer>();
            Vector2 size = renderer.bounds.size;          
            rb.centerOfMass = new Vector2(0.0f, -size.y * (1 - centerOfMassOffset));
            rb.isKinematic = false; // 物理演算を有効化
        }

        // マトリョーシカの状態を取得
        matryoshkaState = GetComponent<CharaState>();
    }

    void FixedUpdate()
    {
        // 無効化されていれば有効にする
        if (rb.isKinematic) { rb.isKinematic = false; Debug.Log("無効化"); }
        // 左右移動入力
        float moveInput = Input.GetAxis("Horizontal");
        // 入力値のデッドゾーンを適用
        if (Mathf.Abs(moveInput) < inputDeadZone){ moveInput = 0.0f; }
        // 移動量
        float speed = 0.0f;

        // 飛んでいるときは移動速度そのままで飛ばせる処理---------------------------------------------------------------
        if (matryoshkaState.state == CharaState.State.Flying)
        {
            // そのままで速度をセット
            speed = rb.velocity.x;
        }

        // 通常時は移動処理を行う------------------------------------------------------------------------------------
        else if (matryoshkaState.state == CharaState.State.Normal)
        {
            // 移動中
            if (moveInput != 0.0f)
            {
                Move(moveInput);
            }
            // 止まった時
            else
            {
                Stopped();
            }

            // 速度を計算
            speed = moveInput * moveSpeed;
        }
        // 死んでいるときは止まる処理だけ行う--------------------------------------------------------------------------
        else if (matryoshkaState.state == CharaState.State.Dead)
        {
            bool isStopped = Stopped();
            // 速度を計算
            speed = rb.velocity.x;

            // 止まった時このスクリプトを無効化する
            if (isStopped)
            {
                rb.isKinematic = false; // 物理演算を有効化
                enabled = false;
                return;
            }
        }

        rb.velocity = new Vector2(speed, rb.velocity.y);
        Debug.Log(matryoshkaState.state);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 死んでいるとき以外で
        if (matryoshkaState.state != CharaState.State.Dead)
        {
            // 地面と当たった時
            if (collision.gameObject.CompareTag("ground"))
            {
                // 状態を「通常」に
                matryoshkaState.SetCharaState(CharaState.State.Normal);
            }
        }
    }

    /**
     *  @brief  マトリョーシカが移動中の処理
     *  @param  float _moveInput          移動している向き
    */
    private void Move(float _moveInput)
    {
        // 移動方向と逆に傾ける
        float tilt = _moveInput * tiltAmount;
        tilt = Mathf.Clamp(tilt, -tiltAmount, tiltAmount);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, tilt);

        tiltVelocity = 0.0f;    // 傾きの速度をリセット
        swingCount = 0;         // 揺れの回数をリセット
        isInDeadZone = false;   // デッドゾーンフラグをリセット
        if (rb.isKinematic) { rb.isKinematic = false; } // 物理演算を有効化
        Debug.Log("有効化");
    }

    /**
     *  @brief  マトリョーシカが止まった時の処理
     *  @return bool true:動きが完全に止まった
    */
    private bool Stopped()
    {
        // 戻したい角度と現在の角度
        float targetRotation = 0.0f;
        float currentRotation = transform.rotation.eulerAngles.z;

        // 角度を-180度から180度の範囲に変換して目標角度との差を出す
        if (currentRotation > 180.0f) currentRotation -= 360.0f;
        float deltaRotation = targetRotation - currentRotation;

        // 反動で揺れてから真っ直ぐに戻る
        tiltVelocity += deltaRotation * returnSpeed * Time.deltaTime;

        // 角度を計算
        float newRotation = currentRotation + tiltVelocity;
        newRotation = Mathf.Clamp(newRotation, -tiltAmount, tiltAmount);

        // 角度がデッドゾーン内にあるかどうかをチェック
        if (Mathf.Abs(deltaRotation) < angleSwingZone)
        {
            if (!isInDeadZone)
            {
                // デッドゾーンを通過したら1回「揺れた」
                swingCount++;
                isInDeadZone = true;
            }
        }
        else { isInDeadZone = false; }

        // 3回目の揺れが終わった時
        if (swingCount >= maxSwimg)
        {
            // 回転、速度などをリセット
            newRotation = 0.0f;
            tiltVelocity = 0.0f;
            rb.velocity = Vector2.zero; 
            rb.angularVelocity = 0.0f;  
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            swingCount = maxSwimg;

            // 物理演算を無効化
            if (!rb.isKinematic) { rb.isKinematic = true; }

            // Rigidbodyを完全に停止
            rb.Sleep();

            return true;   // 動きが止まった
        }

        // 角度のセット
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, newRotation);

        // 段々ふり幅を小さくする
        tiltVelocity *= (1 - damping);

        return false;   // まだ動いている
    }
}