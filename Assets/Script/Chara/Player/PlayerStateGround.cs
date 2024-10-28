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
    private PreventBounce preventBounce = null;                                     // 跳ね防止スクリプト
    private Animator animator = null;                                               // プレイヤーのアニメーター

    private float blinkTimeCount = 0.0f;    // 瞬きをする間隔時間をカウント
    private float sleepTimeCount = 0.0f;    // 寝る間隔時間をカウント

    private float blinkCount = 5.0f;        // 瞬きする間隔

    private float sleepCount = 10.0f;       // 眠る間隔
    private bool isSleep = false;           // true:寝た


    /**
     * @brief 	この状態に入るときに行う関数
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

        // 跳ね防止スクリプトを有効に
        this.preventBounce = _playerMove.GetComponent<PreventBounce>();
        if (!this.preventBounce)
        {
            Debug.LogError("PreventBounceを取得できませんでした。");
            return;
        }

        // 跳ね防止スクリプトを有効に
        this.preventBounce.enabled = true;
        Debug.Log("跳ね防止有効");

        // プレイヤーのアニメーター
        this.animator = _playerMove.GetComponent<Animator>();
        if (!this.animator)
        {
            Debug.LogError("Animatorを取得できませんでした。");
            return;
        }

        // 攻撃成功アニメーション
        if (_playerMove.attackState == PlayerState.AttackState.Success)
        {
            this.animator.SetTrigger("hitTrigger");
        }

        // 攻撃不発時アニメーション
        else if (_playerMove.attackState == PlayerState.AttackState.Failed)
        {
            this.animator.SetTrigger("nonHitTrigger");
        }

        // 攻撃状態リセット
        _playerMove.SetAttackState(PlayerState.AttackState.None);
    }

    /**
     * @brief 	この状態から出るときに行う関数
    */
    public override void Exit() 
    {
        // 跳ね防止スクリプトを無効に
        if (this.preventBounce.enabled) { this.preventBounce.enabled = false; }
        Debug.Log("跳ね防止無効");
    }

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

            // 通常アニメーションのときカウントリセット
            if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
               // Debug.Log("通常アニメーション中");

                this.blinkTimeCount = 0.0f;
                this.sleepTimeCount = 0.0f;
                this.isSleep = false;
            }
        }
        // 止まった時
        else
        {
            // アニメーション
            this.PlayerAnimation();

            // 反動を消す
            if (this.rb.velocity.normalized.x != 0.0f)
            {          
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
    public override void CollisionEnter(Collider2D _collision) { }

    private void PlayerAnimation()
    {
        this.sleepTimeCount += Time.deltaTime;
        this.blinkTimeCount += Time.deltaTime;

        // 眠り優先
        if (!this.isSleep)
        {
            // 眠るアニメーション
            if (this.sleepTimeCount > this.sleepCount)
            {
                this.isSleep = false;
                this.animator.SetTrigger("sleepTrigger");

                // アニメーションの間隔をリセット
                this.sleepTimeCount = 0.0f;
                this.blinkTimeCount = 0.0f;
            }
        }

        // 瞬きアニメーション
        if (!this.isSleep && this.blinkTimeCount > this.blinkCount)
        {
            this.animator.SetTrigger("blinkTrigger");

            // アニメーションの間隔をリセット
            this.blinkTimeCount = 0.0f; 
        }
    }
}
