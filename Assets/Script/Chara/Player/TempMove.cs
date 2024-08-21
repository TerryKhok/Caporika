using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

/**
 *  @brief 	�v���C���[�̈ړ�(�ꎞ�I)
 *  
 *  @memo   �E�ړ������Ɣ��΂̌����ɌX��
 *          �E�~�܂������w��񐔗h��Ď~�܂�
 *          �E���ł���Ƃ��͂������ړ��ł���
 *          
 *          �~�܂��Ă���Ƃ���isKinematic�𖳌��ɂ��Ă���
 *          
 *          �V����PlayerMove�����������炱�̃X�N���v�g�͏���
*/
public class TempMove : MonoBehaviour
{
    protected float moveFactor = 1.0f;              // �v���C���[�̑S�̂̓����𒲐�����W��(0.0f�`1.0f)�A1.0f�̎�100%�͂��e�������

    private float moveSpeed = 2.0f;
    private Rigidbody2D rb = null;
    protected float inputDeadZone = 0.1f;               // ���͂̃f�b�h�]�[��


    private void Start()
    {
        this.rb=GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // ���E�ړ�����
        float moveInput = Input.GetAxis("Horizontal");
        // ���͒l�̃f�b�h�]�[����K�p
        if (Mathf.Abs(moveInput) < this.inputDeadZone) { moveInput = 0.0f; }

        // ���x���v�Z
        float speed = moveInput * moveSpeed * this.moveFactor;
        this.rb.AddForce(new Vector2(speed, 0.0f), ForceMode2D.Force);
    }
}