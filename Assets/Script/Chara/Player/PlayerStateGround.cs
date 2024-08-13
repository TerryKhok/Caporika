using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/**
 * @brief 	プレイヤーが「地面に立っている状態」の処理を行うクラス
 * 
 *  @memo   ・PlayerStateを基底クラスに持つ
 *          ・プレイヤーの状態は、後に実装するPlayerMove.cs内で列挙型(CharaCondition、PlayerCondition)を使用して切り替える
 *          
 *          ・通常スピードで左右移動
 *          ・進む方向と逆向きに傾く
 *          ・止まった時指定回数左右に揺れて止まる
 *          
 *  ========================================================================================================
 *  
 *          MonoBehaviourを継承しないためパラメータは直接いじってください！！！！！！！！！
 *          
 *  ========================================================================================================
*/

public class PlayerStateGround : PlayerState
{
    //===============================================
    //          マトリョーシカの移動
    //===============================================

    private const float moveSpeed = 5.0f;          // 移動速度
    private const float inputDeadZone = 0.1f;      // 入力のデッドゾーン
    private const float returnSpeed = 0.5f;        // 回転を元に戻す速度

    private Rigidbody2D rb;

    /**
     * @brief 	この状態に入るときに行う関数
    */
    public override void Enter(PlayerMove _playerMove)
    {
        if(!_playerMove)
        {
            Debug.LogError("PlayerMoveが存在しません。");
        }

        this.rb = _playerMove.GetComponent<Rigidbody2D>();
        if(!this.rb)
        {
            Debug.LogError("Rigidbody2Dを取得できませんでした。");
            return;
        }
    }

    /**
     * @brief 	この状態から出るときに行う関数
    */
    public override void Exit() { }

    /**
     * @brief 	更新処理
    */
    public override void Update()
    {
        // 左右移動入力
        float moveInput = Input.GetAxis("Horizontal");
        // 入力値のデッドゾーンを適用
        if (Mathf.Abs(moveInput) < inputDeadZone) { moveInput = 0.0f; }

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
                this.rb.AddForce(new Vector2(-this.rb.velocity.normalized.x, 0.0f), ForceMode2D.Impulse);
                Debug.Log(this.rb.velocity.normalized.x);
            }
            Stopped();
        }

        // 速度を計算
        float speed = moveInput * moveSpeed;
        this.rb.AddForce(new Vector2(speed, 0.0f), ForceMode2D.Force);
    }

    /**
     * @brief 	当たった時の処理
     *  @param  Collision _collision    当たったオブジェクト
    */
    public override void CollisionEnter(Collision _collision)
    {
    }

    /**
     *  @brief  マトリョーシカが移動中の処理
     *  @param  float _moveInput          移動している向き
    */
    private void Move(float _moveInput)
    {
        // 移動方向と逆に傾ける
        float tilt = _moveInput * this.tiltAmount;
        tilt = Mathf.Clamp(tilt, -this.tiltAmount, this.tiltAmount);
        this.rb.transform.rotation = Quaternion.Euler(0.0f, 0.0f, tilt);

        this.tiltVelocity = 0.0f;    // 傾きの速度をリセット
        this.swingCount = 0;         // 揺れの回数をリセット
        this.isInDeadZone = false;   // デッドゾーンフラグをリセット
    }

    /**
     *  @brief  マトリョーシカが止まった時の処理
     *  @return bool true:動きが完全に止まった
    */
    private bool Stopped()
    {
        // 戻したい角度と現在の角度
        float targetRotation = 0.0f;
        float currentRotation = this.rb.transform.rotation.eulerAngles.z;

        // 角度を-180度から180度の範囲に変換して目標角度との差を出す
        if (currentRotation > 180.0f) currentRotation -= 360.0f;
        float deltaRotation = targetRotation - currentRotation;

        // 反動で揺れてから真っ直ぐに戻る
        this.tiltVelocity += deltaRotation * returnSpeed * Time.deltaTime;

        // 角度を計算
        float newRotation = currentRotation + this.tiltVelocity;
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
        else { isInDeadZone = false; }

        // 3回目の揺れが終わった時
        if (swingCount >= maxSwimg)
        {
            // 回転、速度などをリセット
            newRotation = 0.0f;
            this.tiltVelocity = 0.0f;
            this.rb.velocity = Vector2.zero;
            this.rb.angularVelocity = 0.0f;
            this.rb.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            this.swingCount = maxSwimg;

            return true;   // 動きが止まった
        }

        // 角度のセット
        this.rb.transform.rotation = Quaternion.Euler(0.0f, 0.0f, newRotation);

        // 段々ふり幅を小さくする
        this.tiltVelocity *= (1 - damping);

        return false;   // まだ動いている
    }
}
