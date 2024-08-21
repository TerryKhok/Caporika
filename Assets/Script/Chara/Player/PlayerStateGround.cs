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
        if (Mathf.Abs(moveInput) < this.inputDeadZone) { moveInput = 0.0f; }

        // 移動中
        if (moveInput != 0.0f)
        {
            this.Move(moveInput);
        }
        // 止まった時
        else
        {
            if (this.rb.velocity.normalized.x != 0.0f)
            {
                // 反動を消す
                this.rb.AddForce(new Vector2(-(this.rb.velocity.x* this.moveDamping), 0.0f), ForceMode2D.Impulse);

            }

            // もし揺れも完全に止まったら「止まった」
            this.isStopped = Stopped();
        }

        // 速度を計算
        float speed = moveInput * moveSpeed;
        this.rb.AddForce(new Vector2(speed, 0.0f), ForceMode2D.Force);
    }

    /**
     * @brief 	当たった時の処理
     *  @param  Collider2D _collision    当たったオブジェクト
    */
    public override void CollisionEnter(Collider2D _collision)
    {
    }
}
