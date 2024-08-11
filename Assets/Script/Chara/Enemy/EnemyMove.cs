using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief 	�G�̊�{�ړ�����
*/
public class EnemyMove : MonoBehaviour
{
    public float moveSpeed;         // �G�̈ړ����x

    private float direction = 1;    // �G�̈ړ��������
    Rigidbody2D rb = null;          // �G��Rigidbody2D
    TenpState enemyState = null;   // �G�̏��

    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        enemyState = GetComponent<TenpState>();
    }

    private void FixedUpdate()
    {
        // ��Ԗ��ɓ�����ς���
        switch (enemyState.state)
        {
            case TenpState.State.Normal:
                // �ȒP�ȍ��E�ړ�
                rb.velocity = new Vector2(moveSpeed * direction, rb.velocity.y);
                break;

            case TenpState.State.Dead:
                // ����
                break;

            default:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �ǂɓ���������ړ�����������t�]����
        if(collision.gameObject.CompareTag("Wall"))
        {
            direction *= -1;
        }
    }
}