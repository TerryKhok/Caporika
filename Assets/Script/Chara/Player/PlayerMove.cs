using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  @brief 	プレイヤーの動きをまとめたクラス
*/
public class PlayerMove : MonoBehaviour
{

    public float centerOfMassOffset = 0.6f; // 重心の位置の割合
    private Rigidbody2D rb;
    private Collider2D trigger;

    /**
     *  @brief 	プレイヤー特有の状態の列挙型
    */

    public enum PlayerCondition
    {
        Ground,     // 地面にいる
        Flying,     // 飛んでいる
        Swimming,   // 水の中にいる
        Dead,       // 死んでいる

        Damaged,    // ダメージを受けている
    }

    public PlayerCondition playerCondition;             // プレイヤー特有の状態
    private PlayerState currentState = null;            // プレイヤーの現在の状態の動き


    void Start()
    {
        // マトリョーシカの重心を下に設定
        this.rb = GetComponent<Rigidbody2D>();
        if (this.rb != null)
        {
            Renderer renderer = GetComponent<Renderer>();
            Vector2 size = renderer.bounds.size;
            this.rb.centerOfMass = new Vector2(0.0f, -size.y * (1 - this.centerOfMassOffset));
        }

        // プレイヤーの状態に合わせて現在の動きを設定
        switch (this.playerCondition)
        {
            case PlayerCondition.Ground:
                this.currentState = new PlayerStateGround();
                break;
            case PlayerCondition.Flying:
                this.currentState = new PlayerStateFlying();
                break;
            case PlayerCondition.Swimming:
                this.currentState = new PlayerStateSwimming();
                break;
            case PlayerCondition.Dead:
                break;
            case PlayerCondition.Damaged:
                break;
            default:
                this.currentState = null;
                break;
        }

        if (this.currentState != null)
        {
            // 変更後の状態の開始処理を行う
            this.currentState.Enter(this);
        }
    }


    private void FixedUpdate()
    {
        this.currentState.Update();
        this.currentState.CollisionEnter(trigger);

        // 水に入っている
        if (trigger && trigger.CompareTag("water"))
        {
            ChangePlayerCondition(PlayerCondition.Swimming);
        }
        else
        {
            ChangePlayerCondition(PlayerCondition.Ground);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        trigger = collision;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        trigger = collision;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        trigger = null;
    }


    /**
     *  @brief 	プレイヤーの状態遷移を行う処理
     *  @param  PlayerCondition _changeCondition    変更後のプレイヤーの状態
    */
    void    ChangePlayerCondition(PlayerCondition _changeCondition)
    {
        // 状態が同じなら処理を行わない
        if(this.playerCondition == _changeCondition) { return; }
        this.playerCondition = _changeCondition;

        if (this.currentState != null)
        {
            // 現在の状態の終了処理を行う
            this.currentState.Exit();
        }

        // 変更する状態の動作クラスにする
        switch (this.playerCondition)
        {
            case PlayerCondition.Ground:
                this.currentState = new PlayerStateGround();
                break;
            case PlayerCondition.Flying:
                this.currentState = new PlayerStateFlying();
                break;
            case PlayerCondition.Swimming:
                this.currentState = new PlayerStateSwimming();
                break;
            case PlayerCondition.Dead:
                break;
            case PlayerCondition.Damaged:
                break;
            default:
                this.currentState = null;
                break;
        }
        Debug.Log(this.playerCondition + "に変更");

        if (this.currentState != null)
        {
            // 変更後の状態の開始処理を行う
            this.currentState.Enter(this);
        }
    }
}
