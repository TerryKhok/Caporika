using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

/**
 *  @brief 	プレイヤーの移動(一時的)
 *  
 *  @memo   試作用
*/
public class TempMove : MonoBehaviour
{
    protected float moveFactor = 1.0f;              // プレイヤーの全体の動きを調整する係数(0.0f～1.0f)、1.0fの時100%力が影響される

    public Rigidbody2D rb; 
    public Rigidbody2D rb2;
    public GameObject obj;
    public float centerOfMassOffset = 0.1f;             // 重心の位置の割合
    protected float inputDeadZone = 0.1f;               // 入力のデッドゾーン
    protected float moveDamping = 0.8f;                 // 減衰係数(どのくらいずつ反動を減らしていくか)
    protected const float moveSpeed = 5.0f;             // 移動速度


    //===============================================
    //          マトリョーシカの揺れ
    //===============================================

    protected float returnSpeed = 2.0f;                // 回転を元に戻す速度
    protected float tiltVelocity = 0.0f;                // 傾きの速度
    protected float tiltAmount = 60.0f;                // 回転角の最大値
    protected float damping = 0.9f;                    // 減衰係数(どのくらいずつ回転角度を減らしていくか)

    protected int maxSwimg = 3;                        // 何回揺れるか 
    protected int swingCount = 0;                     // 揺れの回数をカウント
    protected float angleSwingZone = 1.0f;            // 揺れた判定内かどうか
    protected bool isInDeadZone = false;                // 揺れのデッドゾーン内にいるかどうか

    protected bool isStopped = false;                   // true:止まった

    private void Start()
    {
        if (this.obj != null)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            rb2= obj.GetComponent<Rigidbody2D>();
            Vector2 size = renderer.bounds.size;
            // 重心をオブジェクトの高さのcenterOfMassOffsetの位置に設定
            this.rb2.centerOfMass = new Vector2(0.0f, -size.y * (0.5f - this.centerOfMassOffset));
        }
    }

    private void FixedUpdate()
    {
        // 左右移動入力
        float moveInput = Input.GetAxis("Horizontal");
        // 入力値のデッドゾーンを適用
        if (Mathf.Abs(moveInput) < this.inputDeadZone) { moveInput = 0.0f; }

        // 移動中
        if (moveInput != 0.0f)
        {
            Move(moveInput);
        }
        // 止まった時
        else
        {
            if (this.rb.velocity.normalized.x != 0.0f)
            {
                // 反動を消す
                this.rb.AddForce(new Vector2(-(this.rb.velocity.x * this.moveDamping), 0.0f), ForceMode2D.Impulse);

            }
            // もし揺れも完全に止まったら「止まった」
            this.isStopped = Stopped();
        }

        // 速度を計算
        float speed = moveInput * moveSpeed;
        this.rb.AddForce(new Vector2(speed, 0.0f), ForceMode2D.Force);
    }

    /**
*  @brief  マトリョーシカが移動中の処理
*  @param  float _moveInput          移動している向き
*/
    protected void Move(float _moveInput)
    {
        // 移動方向と逆に傾ける
        float tilt = _moveInput * this.tiltAmount;
        tilt = Mathf.Clamp(tilt, -this.tiltAmount, this.tiltAmount);
        this.rb2.transform.rotation = Quaternion.Euler(0.0f, 0.0f, tilt);

        this.tiltVelocity = 0.0f;    // 傾きの速度をリセット
        this.swingCount = 0;         // 揺れの回数をリセット
        this.isInDeadZone = false;   // デッドゾーンフラグをリセット
    }


    /**
     *  @brief  マトリョーシカが止まった時の処理
     *  @return bool true:動きが完全に止まった
    */
    protected bool Stopped()
    {
        // 戻したい角度と現在の角度
        float targetRotation = 0.0f;
        float currentRotation = this.rb2.transform.rotation.eulerAngles.z;

        // 角度を-180度から180度の範囲に変換して目標角度との差を出す
        if (currentRotation > 180.0f) currentRotation -= 360.0f;
        float deltaRotation = targetRotation - currentRotation;

        // 反動で揺れてから真っ直ぐに戻る
        this.tiltVelocity += deltaRotation * this.returnSpeed * Time.deltaTime;
        // 角度を計算
        float newRotation = currentRotation + this.tiltVelocity * this.moveFactor;
        newRotation = Mathf.Clamp(newRotation, -this.tiltAmount, this.tiltAmount);

        // 角度がデッドゾーン内にあるかどうかをチェック
        if (Mathf.Abs(deltaRotation) < this.angleSwingZone)
        {
            if (!this.isInDeadZone)
            {
                // デッドゾーンを通過したら1回「揺れた」
                this.swingCount++;
                this.isInDeadZone = true;
            }
        }
        else { this.isInDeadZone = false; }

        // 3回目の揺れが終わった時
        if (this.swingCount >= this.maxSwimg)
        {
            // 回転、速度などをリセット
            newRotation = 0.0f;
            this.tiltVelocity = 0.0f;
            this.rb2.angularVelocity = 0.0f;
            this.rb2.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            this.swingCount = this.maxSwimg;

            return true;    // 動きが止まった
        }

        // 角度のセット
        this.rb2.transform.rotation = Quaternion.Euler(0.0f, 0.0f, newRotation);

        // 段々ふり幅を小さくする
        this.tiltVelocity *= (1 - this.damping);

        return false;       // まだ動いている
    }
}