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

    public PlayerState.AttackState attackState;             // プレイヤーの攻撃の状態

    public PlayerState.PlayerCondition playerCondition;     // プレイヤー特有の状態
    private PlayerState currentState = null;                // プレイヤーの現在の状態の動き

    private bool isGoal = false;        // true:ゴールしている
    private bool isInWater = false;     // true:水の中にいる
    private bool isGround = false;      // true:地面と当たっている

    void Start()
    {
        // マトリョーシカの重心を下に設定
        this.rb = GetComponent<Rigidbody2D>();
        if (this.rb != null)
        {
            Renderer renderer = GetComponent<Renderer>();
            Vector2 boundsSize = renderer.localBounds.size;
            Vector3 scale = this.transform.localScale;
            // 重心をオブジェクトの高さのcenterOfMassOffsetの位置に設定
            // 一番下を基準として、centerOfMassOffsetの割合の位置に重心を置く
            var centerOfMass = new Vector2(0.0f, scale.y * ((-boundsSize.y * 0.5f) + (boundsSize.y * this.centerOfMassOffset)));
            this.rb.centerOfMass = centerOfMass;
            //Debug.Log("CenterOfMass" + centerOfMass);
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
            case PlayerState.PlayerCondition.Goal:
                this.currentState = new PlayerStateGoal();
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
        this.currentState.CollisionEnter(this.trigger);

        // ゴールしたら何も行わない
        if (this.isGoal)
        {
            this.ChangePlayerCondition(PlayerState.PlayerCondition.Goal);
            return;
        }

        // 死んでいるとき「死んでいる状態」
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

        // 飛んでいるとき「飛んでいる状態」
        else
        {
            this.ChangePlayerCondition(PlayerState.PlayerCondition.Flying);
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ゴール地点についたとき
        if (collision.gameObject.CompareTag("goal"))
        {
            this.isGoal = true;
        }
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
            if (collision.gameObject.CompareTag("realGround") || collision.gameObject.CompareTag("Button"))
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
        if (collision.gameObject.CompareTag("realGround"))
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
     *  @brief 	マトリョーシカの攻撃状態を設定する
     *  @param  PlayerState.AttackState _attackState    変更後のプレイヤーの攻撃状態
    */
    public void SetAttackState(PlayerState.AttackState _attackState)
    {
        this.attackState = _attackState;
    }

    /**
     *  @brief 	プレイヤーの状態遷移を行う処理
     *  @param  PlayerState.PlayerCondition _changeCondition    変更後のプレイヤーの状態
    */
    public void ChangePlayerCondition(PlayerState.PlayerCondition _changeCondition)
    {
        // 状態が同じなら処理を行わない
        if (this.playerCondition == _changeCondition) { return; }
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
            case PlayerState.PlayerCondition.Goal:
                this.currentState = new PlayerStateGoal();
                break;
            case PlayerState.PlayerCondition.Damaged:
                this.currentState = new PlayerStateDamaged();  
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