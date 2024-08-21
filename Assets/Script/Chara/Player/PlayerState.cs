using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.UIElements;

/**
 * @brief 	プレイヤーの状態毎の処理を行う基底クラス
 * 
 *  @memo   ・プレイヤーの状態は、後に実装するPlayerMove.cs内で列挙型(CharaCondition、PlayerCondition)を使用して切り替える
 *  
 *  ========================================================================================================
 *  
 *          MonoBehaviourを継承しないためパラメータは直接いじってください！！！！！！！！！
 *          
 *  ========================================================================================================       
*/
public abstract class PlayerState
{
    /**
     *  @brief 	プレイヤー特有の状態の列挙型
    */
    public enum PlayerCondition
    { 
        Ground,     // 地面にいる
        Flying,     // 飛んでいる
        Swimming,   // 水の中にいる
        Dead,       // 死んでいる

        Normal,     // 通常状態
        Damaged,    // ダメージを受けている
    }

    protected float moveFactor = 1.0f;              // プレイヤーの全体の動きを調整する係数(0.0f～1.0f)、1.0fの時100%力が影響される

    //===============================================
    //          マトリョーシカの移動
    //===============================================

    protected const float moveSpeed = 5.0f;             // 移動速度
    protected float inputDeadZone = 0.1f;               // 入力のデッドゾーン
    protected float moveDamping = 0.8f;                 // 減衰係数(どのくらいずつ反動を減らしていくか)
    protected Rigidbody2D rb;

    //===============================================
    //          マトリョーシカの揺れ
    //===============================================

    protected GameObject visualTriggerObject;           // 子オブジェクト「VisualTrigger」

    protected  float returnSpeed = 30.0f;                // 回転を元に戻す速度
    protected float tiltVelocity = 0.0f;                // 傾きの速度
    protected  float tiltAmount = 60.0f;                // 回転角の最大値
    protected  float damping = 0.8f;                    // 減衰係数(どのくらいずつ回転角度を減らしていくか)

    protected  int maxSwimg = 3;                        // 何回揺れるか 
    protected 　int swingCount = 0;                     // 揺れの回数をカウント
    protected 　float angleSwingZone = 1.0f;            // 揺れた判定内かどうか
    protected bool isInDeadZone = false;                // 揺れのデッドゾーン内にいるかどうか

    protected bool isStopped = false;                   // true:止まった

    private float targetRotation = 0.0f;                // 目標角度
    private bool isReached = true;                        // true:目標角度に到達した

    /**
     * @brief 	この状態に入るときに行う関数
     * @paraam  PlayerMove _playerMove  
     * 
     * memo    RigidBody2Dやその他コンポーネントを取得するためのみに使用する
    */
    public abstract void Enter(PlayerMove _playerMove);

    /**
     * @brief 	この状態から出るときに行う関数
    */
    public abstract void Exit();

    /**
     * @brief 	更新処理t
    */
    public abstract void Update();

    /**
     * @brief 	当たった時の処理
     *  @param  Collider2D _collision    当たったオブジェクト
    */
    public abstract void CollisionEnter(Collider2D _collision);

    ///**
    // * @brief 	当たらなくなった時の処理
    // *  @param  Collision _collision    当たったオブジェクト
    //*/
    //public abstract void CollisionExit(Collision _collision);

    /**
     *  @brief  マトリョーシカが移動中の処理
     *  @param  float _moveInput          移動している向き
    */
    protected void Move(float _moveInput)
    {
        // 移動方向と逆に傾ける
        float tilt = _moveInput * this.tiltAmount;
        tilt = Mathf.Clamp(tilt, -this.tiltAmount, this.tiltAmount);
        this.visualTriggerObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, tilt);

        this.tiltVelocity = 0.0f;    // 傾きの速度をリセット
        this.swingCount = 0;         // 揺れの回数をリセット
        this.isInDeadZone = false;   // デッドゾーンフラグをリセット
    }

    protected bool Stopped()
    {
        if (this.isReached)
        {
            // 目標角度の設定
            this.CalculateTargetAngle();
            this.isReached = false;
        }

        // 現在の角度
        float currentRotation = this.visualTriggerObject.transform.rotation.eulerAngles.z;

        // 角度を-180度から180度の範囲に変換
        if (currentRotation > 180.0f) currentRotation -= 360.0f;

        // 差を出す
        float deltaRotation = this.targetRotation - currentRotation;

        // 角速度を出す
        this.tiltVelocity += deltaRotation * this.returnSpeed * Time.deltaTime;

        // 更新後の角度
        float newRotation = currentRotation + this.tiltVelocity * this.moveFactor;
        newRotation = Mathf.Clamp(newRotation, -this.tiltAmount, this.tiltAmount);

        // 目標角度に到達していれば「到達した」
        if (Mathf.Abs(newRotation) > Mathf.Abs(this.targetRotation))
        {
            this.isReached = true;
        }

        // 揺れた回数をカウント
        if (Mathf.Abs(deltaRotation) < this.angleSwingZone)
        {
            if (!this.isInDeadZone)
            {
                this.swingCount++;
                this.isInDeadZone = true;
            }
        }
        else
        {
            this.isInDeadZone = false;
        }

        // 指定回数揺れたら
        if (this.swingCount >= this.maxSwimg)
        {
            // 全てリセット
            newRotation = 0.0f;
            this.tiltVelocity = 0.0f;
            this.visualTriggerObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            this.swingCount = this.maxSwimg;

            return true; // 止まった
        }

        // 回転
        this.visualTriggerObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, newRotation);

        // 速度を減らす
        this.tiltVelocity *= 0.99f;

        return false; // 止まっていない
    }

    private void CalculateTargetAngle()
    {
        // 戻したい角度と現在の角度
        float currentRotation = this.visualTriggerObject.transform.rotation.eulerAngles.z;

        // 角度を-180度から180度の範囲に変換して0度との差を出す
        if (currentRotation > 180.0f) currentRotation -= 360.0f;

        Debug.Log("差：" + currentRotation);

        // 目標角度を反対方向に設定
        this.targetRotation = -currentRotation * this.damping;
    }
    /**
     *  @brief  マトリョーシカが完全に動作を停止したかを返す関数
     *  @return bool this.isStopped true:動きが完全に止まった
    */
    public bool GetObjectStopped()
    {
        return this.isStopped;
    }
}