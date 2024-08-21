using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief 	プレイヤーが「死んでいる状態」の処理を行うクラス
 * 
 *  @memo   ・PlayerStateを基底クラスに持つ
 *          ・プレイヤーの状態は、後に実装するPlayerMove.cs内で列挙型(CharaCondition、PlayerCondition)を使用して切り替える
 *          
 *          ・止まった時の反動、揺れる処理を行う
 *          
 *  ========================================================================================================
 *  
 *          MonoBehaviourを継承しないためパラメータは直接いじってください！！！！！！！！！
 *          
 *  ========================================================================================================
*/
public class PlayerStateDead : PlayerState
{
    /**
     * @brief 	この状態に入るときに行う関数
     * @paraam  PlayerMove _playerMove  
     * 
     * @memo    RigidBody2Dやその他コンポーネントを取得するためのみに使用する
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
    }

    /**
     * @brief 	この状態から出るときに行う関数
    */
    public override void Exit()
    {
    }

    /**
     * @brief 	更新処理
    */
    public override void Update()
    {
        if (this.rb.velocity.normalized.x != 0.0f)
        {
            // 反動を消す
            this.rb.AddForce(new Vector2(-(this.rb.velocity.x * this.moveDamping), 0.0f), ForceMode2D.Impulse);

        }

        // もし揺れも完全に止まったら「止まった」
        //this.isStopped = Stopped(moveInput);
    }

    /**
     * @brief 	当たった時の処理
     *  @param  Collider2D _collision    当たったオブジェクト
    */
    public override void CollisionEnter(Collider2D _collision)
    { }
}
