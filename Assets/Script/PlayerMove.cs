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
        if (rb.isKinematic) { rb.isKinematic = false; }
        // 左右移動入力
        float moveInput = Input.GetAxis("Horizontal");

        // 入力値のデッドゾーンを適用
        if (Mathf.Abs(moveInput) < inputDeadZone){ moveInput = 0.0f; }

        // 移動速度
        float speed = 0.0f;
        if (matryoshkaState.state == CharaState.State.Flying) { speed = slowSpeed; }   // 飛んでいるときはゆっくり動く
        else if(matryoshkaState.state == CharaState.State.Dead) { speed = 0.0f; }      // 死んでたら動かない
        else { speed = moveSpeed; }

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

        // 速度をセット
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        Debug.Log(matryoshkaState.state);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 地面と当たった時
        if(collision.gameObject.CompareTag("ground"))
        {
            // 状態を「通常」に
            matryoshkaState.SetCharaState(CharaState.State.Normal);
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
        rb.isKinematic = false; // 物理演算を有効化
    }

    /**
     *  @brief  マトリョーシカが止まった時の処理
    */
    private void Stopped() 
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
            // 回転、速度をリセット
            newRotation = 0.0f;
            tiltVelocity = 0.0f;
            rb.velocity = new Vector2(0.0f, 0.0f);
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            swingCount = maxSwimg;

            // 止まった反動でめっちゃ滑るので物理演算を無効化
            rb.isKinematic = true;
        }

        // 角度のセット
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, newRotation);

        // 段々ふり幅を小さくする
        tiltVelocity *= (1 - damping);
    }
}