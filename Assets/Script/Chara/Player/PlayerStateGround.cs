using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/**
 * @brief 	�v���C���[���u�n�ʂɗ����Ă����ԁv�̏������s���N���X
 * 
 *  @memo   �EPlayerState�����N���X�Ɏ���
 *          �E�v���C���[�̏�Ԃ́A��Ɏ�������PlayerMove.cs���ŗ񋓌^(CharaCondition�APlayerCondition)���g�p���Đ؂�ւ���
 *          
 *          �E�ʏ�X�s�[�h�ō��E�ړ�
 *          �E�i�ޕ����Ƌt�����ɌX��
 *          �E�~�܂������w��񐔍��E�ɗh��Ď~�܂�
 *          
 *  ========================================================================================================
 *  
 *          MonoBehaviour���p�����Ȃ����߃p�����[�^�͒��ڂ������Ă��������I�I�I�I�I�I�I�I�I
 *          
 *  ========================================================================================================
*/

public class PlayerStateGround : PlayerState
{
    //===============================================
    //          �}�g�����[�V�J�̈ړ�
    //===============================================

    private const float moveSpeed = 5.0f;          // �ړ����x
    private const float inputDeadZone = 0.1f;      // ���͂̃f�b�h�]�[��
    private const float returnSpeed = 0.5f;        // ��]�����ɖ߂����x

    private Rigidbody2D rb;

    /**
     * @brief 	���̏�Ԃɓ���Ƃ��ɍs���֐�
    */
    public override void Enter(PlayerMove _playerMove)
    {
        if(!_playerMove)
        {
            Debug.LogError("PlayerMove�����݂��܂���B");
        }

        this.rb = _playerMove.GetComponent<Rigidbody2D>();
        if(!this.rb)
        {
            Debug.LogError("Rigidbody2D���擾�ł��܂���ł����B");
            return;
        }
    }

    /**
     * @brief 	���̏�Ԃ���o��Ƃ��ɍs���֐�
    */
    public override void Exit() { }

    /**
     * @brief 	�X�V����
    */
    public override void Update()
    {
        // ���E�ړ�����
        float moveInput = Input.GetAxis("Horizontal");
        // ���͒l�̃f�b�h�]�[����K�p
        if (Mathf.Abs(moveInput) < inputDeadZone) { moveInput = 0.0f; }

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
                this.rb.AddForce(new Vector2(-this.rb.velocity.normalized.x, 0.0f), ForceMode2D.Impulse);
                Debug.Log(this.rb.velocity.normalized.x);
            }
            Stopped();
        }

        // ���x���v�Z
        float speed = moveInput * moveSpeed;
        this.rb.AddForce(new Vector2(speed, 0.0f), ForceMode2D.Force);
    }

    /**
     * @brief 	�����������̏���
     *  @param  Collision _collision    ���������I�u�W�F�N�g
    */
    public override void CollisionEnter(Collision _collision)
    {
    }

    /**
     *  @brief  �}�g�����[�V�J���ړ����̏���
     *  @param  float _moveInput          �ړ����Ă������
    */
    private void Move(float _moveInput)
    {
        // �ړ������Ƌt�ɌX����
        float tilt = _moveInput * this.tiltAmount;
        tilt = Mathf.Clamp(tilt, -this.tiltAmount, this.tiltAmount);
        this.rb.transform.rotation = Quaternion.Euler(0.0f, 0.0f, tilt);

        this.tiltVelocity = 0.0f;    // �X���̑��x�����Z�b�g
        this.swingCount = 0;         // �h��̉񐔂����Z�b�g
        this.isInDeadZone = false;   // �f�b�h�]�[���t���O�����Z�b�g
    }

    /**
     *  @brief  �}�g�����[�V�J���~�܂������̏���
     *  @return bool true:���������S�Ɏ~�܂���
    */
    private bool Stopped()
    {
        // �߂������p�x�ƌ��݂̊p�x
        float targetRotation = 0.0f;
        float currentRotation = this.rb.transform.rotation.eulerAngles.z;

        // �p�x��-180�x����180�x�͈̔͂ɕϊ����ĖڕW�p�x�Ƃ̍����o��
        if (currentRotation > 180.0f) currentRotation -= 360.0f;
        float deltaRotation = targetRotation - currentRotation;

        // �����ŗh��Ă���^�������ɖ߂�
        this.tiltVelocity += deltaRotation * returnSpeed * Time.deltaTime;

        // �p�x���v�Z
        float newRotation = currentRotation + this.tiltVelocity;
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
        else { isInDeadZone = false; }

        // 3��ڂ̗h�ꂪ�I�������
        if (swingCount >= maxSwimg)
        {
            // ��]�A���x�Ȃǂ����Z�b�g
            newRotation = 0.0f;
            this.tiltVelocity = 0.0f;
            this.rb.velocity = Vector2.zero;
            this.rb.angularVelocity = 0.0f;
            this.rb.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            this.swingCount = maxSwimg;

            return true;   // �������~�܂���
        }

        // �p�x�̃Z�b�g
        this.rb.transform.rotation = Quaternion.Euler(0.0f, 0.0f, newRotation);

        // �i�X�ӂ蕝������������
        this.tiltVelocity *= (1 - damping);

        return false;   // �܂������Ă���
    }
}
