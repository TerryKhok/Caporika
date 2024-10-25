using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

/**
 * @brief 	�v���C���[�̏�Ԗ��̏������s�����N���X
 * 
 *  @memo   �E�v���C���[�̏�Ԃ́A��Ɏ�������PlayerMove.cs���ŗ񋓌^(CharaCondition�APlayerCondition)���g�p���Đ؂�ւ���
 *  
 *  ========================================================================================================
 *  
 *          MonoBehaviour���p�����Ȃ����߃p�����[�^�͒��ڂ������Ă��������I�I�I�I�I�I�I�I�I
 *          
 *  ========================================================================================================       
*/
public abstract class PlayerState
{
    /**
     *  @brief 	�v���C���[���L�̏�Ԃ̗񋓌^
    */
    public enum PlayerCondition
    { 
        Ground,     // �n�ʂɂ���
        Flying,     // ���ł���
        Swimming,   // ���̒��ɂ���
        Dead,       // ����ł���
        Goal,       //�S�[�����Ă���

        Normal,     // �ʏ���
        Damaged,    // �_���[�W���󂯂Ă���
    }

    /**
     *  @brief 	�v���C���[���U���ɐ����������ǂ���
    */
    public enum AttackState
    {
        None,           // �����s���Ă��Ȃ�
        Failed,         // ���s����
        Success,        // ��������
    }

    protected float moveFactor = 1.0f;              // �v���C���[�̑S�̂̓����𒲐�����W��(0.0f�`1.0f)�A1.0f�̎�100%�͂��e�������

    //===============================================
    //          �}�g�����[�V�J�̈ړ�
    //===============================================

    protected const float moveSpeed = 8.0f;             // �ړ����x
    protected float inputDeadZone = 0.1f;               // ���͂̃f�b�h�]�[��
    protected float moveDamping = 0.8f;                 // �����W��(�ǂ̂��炢�����������炵�Ă�����)
    protected Rigidbody2D rb = null;

    //===============================================
    //          �}�g�����[�V�J�̗h��
    //===============================================

    protected  float returnSpeed = 0.5f;                // ��]�����ɖ߂����x
    protected float tiltVelocity = 0.0f;                // �X���̑��x
    protected  float tiltAmount = 60.0f;                // ��]�p�̍ő�l
    protected  float damping = 0.8f;                    // �����W��(�ǂ̂��炢����]�p�x�����炵�Ă�����)

    protected  int maxSwimg = 3;                        // ����h��邩 
    protected �@int swingCount = 0;                     // �h��̉񐔂��J�E���g
    protected �@float angleSwingZone = 1.0f;            // �h�ꂽ��������ǂ���
    protected bool isInDeadZone = false;                // �h��̃f�b�h�]�[�����ɂ��邩�ǂ���

    protected bool isStopped = false;                   // true:�~�܂���

    /**
     * @brief 	���̏�Ԃɓ���Ƃ��ɍs���֐�
     * @paraam  PlayerMove _playerMove  
     * 
     * memo    RigidBody2D�₻�̑��R���|�[�l���g���擾���邽�߂݂̂Ɏg�p����
    */
    public abstract void Enter(PlayerMove _playerMove);

    /**
     * @brief 	���̏�Ԃ���o��Ƃ��ɍs���֐�
    */
    public abstract void Exit();

    /**
     * @brief 	�X�V����t
    */
    public abstract void Update();

    /**
     * @brief 	�����������̏���
     *  @param  Collider2D _collision    ���������I�u�W�F�N�g
    */
    public abstract void CollisionEnter(Collider2D _collision);

    ///**
    // * @brief 	������Ȃ��Ȃ������̏���
    // *  @param  Collision _collision    ���������I�u�W�F�N�g
    //*/
    //public abstract void CollisionExit(Collision _collision);

    /**
     *  @brief  �}�g�����[�V�J���ړ����̏���
     *  @param  float _moveInput          �ړ����Ă������
    */
    protected void Move(float _moveInput)
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
    protected bool Stopped()
    {
        // �߂������p�x�ƌ��݂̊p�x
        float targetRotation = 0.0f;
        float currentRotation = this.rb.transform.rotation.eulerAngles.z;

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
            this.rb.angularVelocity = 0.0f;
            this.rb.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            this.swingCount = this.maxSwimg;

            return true;    // �������~�܂���
        }

        // �p�x�̃Z�b�g
        this.rb.transform.rotation = Quaternion.Euler(0.0f, 0.0f, newRotation);

        // �i�X�ӂ蕝������������
        this.tiltVelocity *= (1 - this.damping );

        return false;       // �܂������Ă���
    }

    /**
     *  @brief  �}�g�����[�V�J�����S�ɓ�����~��������Ԃ��֐�
     *  @return bool this.isStopped true:���������S�Ɏ~�܂���
    */
    public bool GetObjectStopped()
    {
        return this.isStopped;
    }
}