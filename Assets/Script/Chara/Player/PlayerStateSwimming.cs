using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief 	プレイヤーが「水の中にいる状態」の処理を行うクラス
 * 
 *  @memo   ・PlayerStateを基底クラスに持つ
 *          ・プレイヤーの状態は、後に実装するPlayerMove.cs内で列挙型(CharaCondition、PlayerCondition)を使用して切り替える
 *          
 *          ・遅めのスピードで左右移動
 *          ・進む方向と逆向きに傾く
 *          ・止まった時指定回数左右に揺れて止まる
 *          
 *  ========================================================================================================
 *  
 *          MonoBehaviourを継承しないためパラメータは直接いじってください！！！！！！！！！
 *          
 *  ========================================================================================================
*/
public class PlayerStateSwimming : PlayerState
{
    private const float swimmingFactor = 0.1f;          // 水にいるときの動作全体での力の影響度合い(0.0f ～ 1.0f)

    /**
     * @brief 	この状態に入るときに行う関数
     * @paraam  PlayerMove _playerMove  
     * 
     * memo    RigidBody2Dやその他コンポーネントを取得するためのみに使用する
    */
    public override void Enter(PlayerMove _playerMove)
    {
        if (!_playerMove)
        {
            Debug.LogError("PlayerMoveが存在しません。");
        }

        this.rb = _playerMove.GetComponent<Rigidbody2D>();
        if (!this.rb)
        {
            Debug.LogError("Rigidbody2Dを取得できませんでした。");
            return;
        }

        // 水の中にいるときの調節係数の設定
        this.moveFactor = swimmingFactor;

        // 水に入った時の衝撃
        this.rb.AddForce(-(this.rb.velocity * 0.3f), ForceMode2D.Impulse);
    }

    /**
     * @brief 	この状態から出るときに行う関数
    */
    public override void Exit()
    {
        // 水から出たときの衝撃
        this.rb.AddForce((this.rb.velocity * 0.3f), ForceMode2D.Impulse);
    }

    /**
     * @brief 	更新処理
    */
    public override void Update()
    {
        Debug.Log("水の中");
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
                this.rb.AddForce(new Vector2(-(this.rb.velocity.x * this.moveDamping * this.moveFactor), 0.0f), ForceMode2D.Impulse);
            }

            // もし揺れも完全に止まったら「止まった」
            this.isStopped = Stopped();
        }

        // 速度を計算
        float speed = moveInput * moveSpeed * this.moveFactor;
        this.rb.AddForce(new Vector2(speed, 0.0f), ForceMode2D.Force);

        // 水の反発力
        this.rb.AddForce(-(this.rb.velocity * (1.0f - this.moveFactor)), ForceMode2D.Force);
    }

    /**
     * @brief 	当たった時の処理
     *  @param  Collider2D _collision    当たったオブジェクト
    */
    public override void CollisionEnter(Collider2D _collision)
    { }
}
