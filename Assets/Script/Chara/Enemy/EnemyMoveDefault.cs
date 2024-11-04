using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief   敵キャラの基本的な移動
 * 
 * @memo    ・地面についている時は左右移動
 *          ・壁に当たると反転
 */
public class EnemyMoveDefault : CharaMove
{
    public float moveSpeed;         // 敵の移動速度

    private float direction = 1;    // 敵の移動する向き
    Rigidbody2D rb = null;          // 敵のRigidbody2D

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // 状態毎に動きを変える
        switch (this.GetCharaCondition())
        {
            case CharaCondition.Ground:
                // 簡単な左右移動
                rb.velocity = new Vector2(moveSpeed * direction, rb.velocity.y);
                break;

            case CharaCondition.Dead:
                // 死んだ
                break;

            default:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 壁に当たったら移動する向きを逆転する
        if (collision.gameObject.CompareTag("Wall"))
        {
            direction *= -1;
            FlipObjectX();
        }
    }

    void FlipObjectX()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1; // x軸を反転
        transform.localScale = scale;
    }
}
