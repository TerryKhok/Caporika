using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief 	プレイヤーが「飛んでいる状態」の処理を行うクラス
 * 
 *  @memo   ・PlayerStateを基底クラスに持つ
 *          ・プレイヤーの状態は、後に実装するPlayerMove.cs内で列挙型(CharaCondition、PlayerCondition)を使用して切り替える
 *          
 *          ・少しだけだが空中で左右移動が可能
 *          
 *  ========================================================================================================
 *  
 *          MonoBehaviourを継承しないためパラメータは直接いじってください！！！！！！！！！
 *          
 *  ========================================================================================================
*/
public class PlayerStateFlying : PlayerState
{

    private const float flyingFactor = 0.1f;            // 空中にいる時の動作全体での力の影響度合い(0.0f ～ 1.0f)
    private Animator animator = null;                   // プレイヤーのアニメーター

    /**
     * @brief 	この状態に入るときに行う関数
     * @paraam  PlayerMove _playerMove  
     * 
     * memo    RigidBody2Dやその他コンポーネントを取得するためのみに使用する
    */
    public override void Enter(PlayerMove _playerMove)
    {
        //Debug.Log("飛んでいる状態" + _playerMove.gameObject.name);
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

        // プレイヤーのアニメーター
        this.animator = _playerMove.GetComponent<Animator>();
        if (!this.animator)
        {
            Debug.LogError("Animatorを取得できませんでした。");
            return;
        }

        // プレイヤーの動作の影響度合い
        this.moveFactor = flyingFactor;

        // 飛んでいるアニメーション
        this.animator.SetTrigger("flyingTrigger");
        //Debug.Log("飛んでいるアニメーション");
    }

    /**
     * @brief 	この状態から出るときに行う関数
    */
    public override void Exit()
    {}

    /**
     * @brief 	更新処理
    */
    public override void Update()
    {
        // 左右移動入力
        float moveInput = Input.GetAxis("Horizontal");
        // 入力値のデッドゾーンを適用
        if (Mathf.Abs(moveInput) < this.inputDeadZone) { moveInput = 0.0f; }

        // 速度を計算
        float speed = moveInput * moveSpeed * this.moveFactor;
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
