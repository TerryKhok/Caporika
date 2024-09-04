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

    public PlayerState.PlayerCondition playerCondition;     // プレイヤー特有の状態
    private PlayerState currentState = null;                // プレイヤーの現在の状態の動き

    private bool isInWater = false;     // true:水の中にいる
    private bool isGround = false;      // true:地面と当たっている

    void Start()
    {
        // マトリョーシカの重心を下に設定
        this.rb = GetComponent<Rigidbody2D>();
        if (this.rb != null)
        {
            Renderer renderer = GetComponent<Renderer>();
            Vector2 size = renderer.bounds.size;
            // 重心をオブジェクトの高さのcenterOfMassOffsetの位置に設定
            this.rb.centerOfMass = new Vector2(0.0f, -size.y * (0.5f - this.centerOfMassOffset));
        }

        // プレイヤーの状態に合わせて現在の動きを設定
        switch (this.playerCondition)
        {
            case PlayerState.PlayerCondition.Ground:
                this.currentState = new PlayerStateGround();
                break;
            case PlayerState.PlayerCondition.Flying:
                this.currentState = new PlayerStateFlying();
                break;
            case PlayerState.PlayerCondition.Swimming:
                this.currentState = new PlayerStateSwimming();
                break;
            case PlayerState.PlayerCondition.Dead:
                this.currentState = new PlayerStateDead();
                break;
            case PlayerState.PlayerCondition.Damaged:
                this.currentState = new PlayerStateDamaged();
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

    void OnDrawGizmos()
    {
        if (rb != null)
        {
            // 重心の位置を赤い球で表示
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(rb.worldCenterOfMass, 0.1f);
        }
    }


    private void FixedUpdate()
    {
        this.currentState.Update();
        this.currentState.CollisionEnter(trigger);

        // 死んでいるとき何も行わない
        if (this.playerCondition == PlayerState.PlayerCondition.Dead)
        {
            return;
        }

        // 水に入っているとき「水の中にいる状態」
        if (this.isInWater)
        {
            this.ChangePlayerCondition(PlayerState.PlayerCondition.Swimming);
            return;
        }

        // 地面と当たっているとき「地面に立っている状態」
        if (this.isGround)
        {
            this.ChangePlayerCondition(PlayerState.PlayerCondition.Ground);
            return;
        }
        else
        {
            this.ChangePlayerCondition(PlayerState.PlayerCondition.Flying);
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("water"))
        {
            this.isInWater = true;
        }

        // 水に入っているときは判定を行わない
        if (!this.isInWater)
        {
            // 地面かボタン
            if (collision.gameObject.CompareTag("ground") || collision.gameObject.CompareTag("Button"))
            {
                this.isGround = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("water"))
        {
            this.isInWater = false;
        }
        // 地面
        if (collision.gameObject.CompareTag("ground"))
        {
            this.isGround = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // 水に入っているときは判定を行わない
        if (!this.isInWater)
        {
            // ボタン
            if (collision.gameObject.CompareTag("Button"))
            { 
                this.isGround = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // ボタン
        if (collision.gameObject.CompareTag("Button"))
        {
            this.isGround = false;
        }
    }

    /**
     *  @brief 	プレイヤーの状態遷移を行う処理
     *  @param  PlayerState.PlayerCondition _changeCondition    変更後のプレイヤーの状態
    */
    public void    ChangePlayerCondition(PlayerState.PlayerCondition _changeCondition)
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
            case PlayerState.PlayerCondition.Ground:
                this.currentState = new PlayerStateGround();
                break;
            case PlayerState.PlayerCondition.Flying:
                this.currentState = new PlayerStateFlying();
                break;
            case PlayerState.PlayerCondition.Swimming:
                this.currentState = new PlayerStateSwimming();
                break;
            case PlayerState.PlayerCondition.Dead:
                this.currentState = new PlayerStateDead();
                break;
            case PlayerState.PlayerCondition.Damaged:
                this.currentState = new PlayerStateDead();  // 一旦死んだことにする
                break;
            default:
                this.currentState = null;
                break;
        }
        //Debug.Log(this.playerCondition + "に変更");

        if (this.currentState != null)
        {
            // 変更後の状態の開始処理を行う
            this.currentState.Enter(this);
        }
    }
}