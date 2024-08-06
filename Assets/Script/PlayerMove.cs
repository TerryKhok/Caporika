using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

/**
 *  @brief 	�v���C���[�̈ړ�
 *  
 *  @memo   �E�ړ������Ɣ��΂̌����ɌX��
 *          �E�~�܂������w��񐔗h��Ď~�܂�
 *          �E���ł���Ƃ��͂������ړ��ł���
 *          
 *          �~�܂��Ă���Ƃ���isKinematic�𖳌��ɂ��Ă���
*/
public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5.0f;          // �ړ����x
    public float slowSpeed = 1.0f;          // ���ł���Ƃ��̑��x
    public float tiltAmount = 45.0f;        // ��]�p�̍ő�l
    public float returnSpeed = 2.0f;        // ��]�����ɖ߂����x
    public float damping = 0.1f;            // �����W��
    public float inputDeadZone = 0.1f;      // ���͂̃f�b�h�]�[��
    public int maxSwimg = 3;                // ����h��邩 
    public float centerOfMassOffset = 0.6f; // �d�S�̈ʒu�̊���

    private Rigidbody2D rb;
    private float tiltVelocity = 0f;        // �X���̑��x
    private int swingCount = 0;             // �h��̉񐔂��J�E���g
    private bool isInDeadZone = false;      // �f�b�h�]�[�����ɂ��邩�ǂ���
    private float angleSwingZone = 1.0f;    // �h�ꂽ��������ǂ���

    CharaState matryoshkaState = null;      // �}�g�����[�V�J�̏��

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // �d�S�����ɐݒ�
        if (rb != null)
        {
            Renderer renderer = GetComponent<Renderer>();
            Vector2 size = renderer.bounds.size;          
            rb.centerOfMass = new Vector2(0.0f, -size.y * (1 - centerOfMassOffset));
            rb.isKinematic = false; // �������Z��L����
        }

        // �}�g�����[�V�J�̏�Ԃ��擾
        matryoshkaState = GetComponent<CharaState>();
    }

    void FixedUpdate()
    {
        // ����������Ă���ΗL���ɂ���
        if (rb.isKinematic) { rb.isKinematic = false; }
        // ���E�ړ�����
        float moveInput = Input.GetAxis("Horizontal");

        // ���͒l�̃f�b�h�]�[����K�p
        if (Mathf.Abs(moveInput) < inputDeadZone){ moveInput = 0.0f; }

        // �ړ����x
        float speed = 0.0f;
        if (matryoshkaState.state == CharaState.State.Flying) { speed = slowSpeed; }   // ���ł���Ƃ��͂�����蓮��
        else if(matryoshkaState.state == CharaState.State.Dead) { speed = 0.0f; }      // ����ł��瓮���Ȃ�
        else { speed = moveSpeed; }

        // �ړ���
        if (moveInput != 0.0f)
        {
            Move(moveInput);
        }
        // �~�܂�����
        else
        {
            Stopped();
        }

        // ���x���Z�b�g
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        Debug.Log(matryoshkaState.state);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �n�ʂƓ���������
        if(collision.gameObject.CompareTag("ground"))
        {
            // ��Ԃ��u�ʏ�v��
            matryoshkaState.SetCharaState(CharaState.State.Normal);
        }
    }

    /**
     *  @brief  �}�g�����[�V�J���ړ����̏���
     *  @param  float _moveInput          �ړ����Ă������
    */
    private void Move(float _moveInput)
    {
        // �ړ������Ƌt�ɌX����
        float tilt = _moveInput * tiltAmount;
        tilt = Mathf.Clamp(tilt, -tiltAmount, tiltAmount);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, tilt);

        tiltVelocity = 0.0f;    // �X���̑��x�����Z�b�g
        swingCount = 0;         // �h��̉񐔂����Z�b�g
        isInDeadZone = false;   // �f�b�h�]�[���t���O�����Z�b�g
        rb.isKinematic = false; // �������Z��L����
    }

    /**
     *  @brief  �}�g�����[�V�J���~�܂������̏���
    */
    private void Stopped() 
    {
        // �߂������p�x�ƌ��݂̊p�x
        float targetRotation = 0.0f;
        float currentRotation = transform.rotation.eulerAngles.z;

        // �p�x��-180�x����180�x�͈̔͂ɕϊ����ĖڕW�p�x�Ƃ̍����o��
        if (currentRotation > 180.0f) currentRotation -= 360.0f;
        float deltaRotation = targetRotation - currentRotation;

        // �����ŗh��Ă���^�������ɖ߂�
        tiltVelocity += deltaRotation * returnSpeed * Time.deltaTime;

        // �p�x���v�Z
        float newRotation = currentRotation + tiltVelocity;
        newRotation = Mathf.Clamp(newRotation, -tiltAmount, tiltAmount);

        // �p�x���f�b�h�]�[�����ɂ��邩�ǂ������`�F�b�N
        if (Mathf.Abs(deltaRotation) < angleSwingZone)
        {
            if (!isInDeadZone)
            {
                // �f�b�h�]�[����ʉ߂�����1��u�h�ꂽ�v
                swingCount++;
                isInDeadZone = true;
            }
        }
        else { isInDeadZone = false; }

        // 3��ڂ̗h�ꂪ�I�������
        if (swingCount >= maxSwimg)
        {
            // ��]�A���x�����Z�b�g
            newRotation = 0.0f;
            tiltVelocity = 0.0f;
            rb.velocity = new Vector2(0.0f, 0.0f);
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            swingCount = maxSwimg;

            // �~�܂��������ł߂����ኊ��̂ŕ������Z�𖳌���
            rb.isKinematic = true;
        }

        // �p�x�̃Z�b�g
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, newRotation);

        // �i�X�ӂ蕝������������
        tiltVelocity *= (1 - damping);
    }
}