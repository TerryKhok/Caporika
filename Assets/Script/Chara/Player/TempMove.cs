using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

/**
 *  @brief 	�v���C���[�̈ړ�(�ꎞ�I)
 *  
 *  @memo   ����p
*/
public class TempMove : MonoBehaviour
{
    protected float moveFactor = 1.0f;              // �v���C���[�̑S�̂̓����𒲐�����W��(0.0f�`1.0f)�A1.0f�̎�100%�͂��e�������

    public Rigidbody2D rb; 
    public Rigidbody2D rb2;
    public GameObject obj;
    public float centerOfMassOffset = 0.1f;             // �d�S�̈ʒu�̊���
    protected float inputDeadZone = 0.1f;               // ���͂̃f�b�h�]�[��
    protected float moveDamping = 0.8f;                 // �����W��(�ǂ̂��炢�����������炵�Ă�����)
    protected const float moveSpeed = 5.0f;             // �ړ����x


    //===============================================
    //          �}�g�����[�V�J�̗h��
    //===============================================

    protected float returnSpeed = 2.0f;                // ��]�����ɖ߂����x
    protected float tiltVelocity = 0.0f;                // �X���̑��x
    protected float tiltAmount = 60.0f;                // ��]�p�̍ő�l
    protected float damping = 0.9f;                    // �����W��(�ǂ̂��炢����]�p�x�����炵�Ă�����)

    protected int maxSwimg = 3;                        // ����h��邩 
    protected int swingCount = 0;                     // �h��̉񐔂��J�E���g
    protected float angleSwingZone = 1.0f;            // �h�ꂽ��������ǂ���
    protected bool isInDeadZone = false;                // �h��̃f�b�h�]�[�����ɂ��邩�ǂ���

    protected bool isStopped = false;                   // true:�~�܂���

    private void Start()
    {
        if (this.obj != null)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            rb2= obj.GetComponent<Rigidbody2D>();
            Vector2 size = renderer.bounds.size;
            // �d�S���I�u�W�F�N�g�̍�����centerOfMassOffset�̈ʒu�ɐݒ�
            this.rb2.centerOfMass = new Vector2(0.0f, -size.y * (0.5f - this.centerOfMassOffset));
        }
    }

    private void FixedUpdate()
    {
        // ���E�ړ�����
        float moveInput = Input.GetAxis("Horizontal");
        // ���͒l�̃f�b�h�]�[����K�p
        if (Mathf.Abs(moveInput) < this.inputDeadZone) { moveInput = 0.0f; }

        // �ړ���
        if (moveInput != 0.0f)
        {
            Move(moveInput);
        }
        // �~�܂�����
        else
        {
            if (this.rb.velocity.normalized.x != 0.0f)
            {
                // ����������
                this.rb.AddForce(new Vector2(-(this.rb.velocity.x * this.moveDamping), 0.0f), ForceMode2D.Impulse);

            }
            // �����h������S�Ɏ~�܂�����u�~�܂����v
            this.isStopped = Stopped();
        }

        // ���x���v�Z
        float speed = moveInput * moveSpeed;
        this.rb.AddForce(new Vector2(speed, 0.0f), ForceMode2D.Force);
    }

    /**
*  @brief  �}�g�����[�V�J���ړ����̏���
*  @param  float _moveInput          �ړ����Ă������
*/
    protected void Move(float _moveInput)
    {
        // �ړ������Ƌt�ɌX����
        float tilt = _moveInput * this.tiltAmount;
        tilt = Mathf.Clamp(tilt, -this.tiltAmount, this.tiltAmount);
        this.rb2.transform.rotation = Quaternion.Euler(0.0f, 0.0f, tilt);

        this.tiltVelocity = 0.0f;    // �X���̑��x�����Z�b�g
        this.swingCount = 0;         // �h��̉񐔂����Z�b�g
        this.isInDeadZone = false;   // �f�b�h�]�[���t���O�����Z�b�g
    }


    /**
     *  @brief  �}�g�����[�V�J���~�܂������̏���
     *  @return bool true:���������S�Ɏ~�܂���
    */
    protected bool Stopped()
    {
        // �߂������p�x�ƌ��݂̊p�x
        float targetRotation = 0.0f;
        float currentRotation = this.rb2.transform.rotation.eulerAngles.z;

        // �p�x��-180�x����180�x�͈̔͂ɕϊ����ĖڕW�p�x�Ƃ̍����o��
        if (currentRotation > 180.0f) currentRotation -= 360.0f;
        float deltaRotation = targetRotation - currentRotation;

        // �����ŗh��Ă���^�������ɖ߂�
        this.tiltVelocity += deltaRotation * this.returnSpeed * Time.deltaTime;
        // �p�x���v�Z
        float newRotation = currentRotation + this.tiltVelocity * this.moveFactor;
        newRotation = Mathf.Clamp(newRotation, -this.tiltAmount, this.tiltAmount);

        // �p�x���f�b�h�]�[�����ɂ��邩�ǂ������`�F�b�N
        if (Mathf.Abs(deltaRotation) < this.angleSwingZone)
        {
            if (!this.isInDeadZone)
            {
                // �f�b�h�]�[����ʉ߂�����1��u�h�ꂽ�v
                this.swingCount++;
                this.isInDeadZone = true;
            }
        }
        else { this.isInDeadZone = false; }

        // 3��ڂ̗h�ꂪ�I�������
        if (this.swingCount >= this.maxSwimg)
        {
            // ��]�A���x�Ȃǂ����Z�b�g
            newRotation = 0.0f;
            this.tiltVelocity = 0.0f;
            this.rb2.angularVelocity = 0.0f;
            this.rb2.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            this.swingCount = this.maxSwimg;

            return true;    // �������~�܂���
        }

        // �p�x�̃Z�b�g
        this.rb2.transform.rotation = Quaternion.Euler(0.0f, 0.0f, newRotation);

        // �i�X�ӂ蕝������������
        this.tiltVelocity *= (1 - this.damping);

        return false;       // �܂������Ă���
    }
}