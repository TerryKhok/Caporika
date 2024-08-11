using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief 	“G‚ÌŠî–{ˆÚ“®ˆ—
*/
public class EnemyMove : MonoBehaviour
{
    public float moveSpeed;         // “G‚ÌˆÚ“®‘¬“x

    private float direction = 1;    // “G‚ÌˆÚ“®‚·‚éŒü‚«
    Rigidbody2D rb = null;          // “G‚ÌRigidbody2D
    TenpState enemyState = null;   // “G‚Ìó‘Ô

    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        enemyState = GetComponent<TenpState>();
    }

    private void FixedUpdate()
    {
        // ó‘Ô–ˆ‚É“®‚«‚ğ•Ï‚¦‚é
        switch (enemyState.state)
        {
            case TenpState.State.Normal:
                // ŠÈ’P‚È¶‰EˆÚ“®
                rb.velocity = new Vector2(moveSpeed * direction, rb.velocity.y);
                break;

            case TenpState.State.Dead:
                // €‚ñ‚¾
                break;

            default:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // •Ç‚É“–‚½‚Á‚½‚çˆÚ“®‚·‚éŒü‚«‚ğ‹t“]‚·‚é
        if(collision.gameObject.CompareTag("Wall"))
        {
            direction *= -1;
        }
    }
}