using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief   �G�L�����̊�{�I�Ȉړ�
 * 
 * @memo    �E�n�ʂɂ��Ă��鎞�͍��E�ړ�
 *          �E�ǂɓ�����Ɣ��]
 */
public class EnemyMoveDefault : CharaMove
{
    public float moveSpeed;         // �G�̈ړ����x

    private float direction = 1;    // �G�̈ړ��������
    Rigidbody2D rb = null;          // �G��Rigidbody2D

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // ��Ԗ��ɓ�����ς���
        switch (this.GetCharaCondition())
        {
            case CharaCondition.Ground:
                // �ȒP�ȍ��E�ړ�
                rb.velocity = new Vector2(moveSpeed * direction, rb.velocity.y);
                break;

            case CharaCondition.Dead:
                // ����
                break;

            default:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �ǂɓ���������ړ�����������t�]����
        if (collision.gameObject.CompareTag("Wall"))
        {
            direction *= -1;
            FlipObjectX();
        }
    }

    void FlipObjectX()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1; // x���𔽓]
        transform.localScale = scale;
    }
}
