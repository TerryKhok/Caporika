using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief 	�v���C���[���u���̒��ɂ����ԁv�̏������s���N���X
 * 
 *  @memo   �EPlayerState�����N���X�Ɏ���
 *          �E�v���C���[�̏�Ԃ́A��Ɏ�������PlayerMove.cs���ŗ񋓌^(CharaCondition�APlayerCondition)���g�p���Đ؂�ւ���
 *          
 *          �E�x�߂̃X�s�[�h�ō��E�ړ�
 *          �E�i�ޕ����Ƌt�����ɌX��
 *          �E�~�܂������w��񐔍��E�ɗh��Ď~�܂�
 *          
 *  ========================================================================================================
 *  
 *          MonoBehaviour���p�����Ȃ����߃p�����[�^�͒��ڂ������Ă��������I�I�I�I�I�I�I�I�I
 *          
 *  ========================================================================================================
*/
public class PlayerStateSwimming : PlayerState
{
    private const float swimmingFactor = 0.1f;          // ���ɂ���Ƃ��̓���S�̂ł̗͂̉e���x����(0.0f �` 1.0f)

    /**
     * @brief 	���̏�Ԃɓ���Ƃ��ɍs���֐�
     * @paraam  PlayerMove _playerMove  
     * 
     * memo    RigidBody2D�₻�̑��R���|�[�l���g���擾���邽�߂݂̂Ɏg�p����
    */
    public override void Enter(PlayerMove _playerMove)
    {
        if (!_playerMove)
        {
            Debug.LogError("PlayerMove�����݂��܂���B");
        }

        this.rb = _playerMove.GetComponent<Rigidbody2D>();
        if (!this.rb)
        {
            Debug.LogError("Rigidbody2D���擾�ł��܂���ł����B");
            return;
        }

        // ���̒��ɂ���Ƃ��̒��ߌW���̐ݒ�
        this.moveFactor = swimmingFactor;

        // ���ɓ��������̏Ռ�
        this.rb.AddForce(-(this.rb.velocity * 0.3f), ForceMode2D.Impulse);
    }

    /**
     * @brief 	���̏�Ԃ���o��Ƃ��ɍs���֐�
    */
    public override void Exit()
    {
        // ������o���Ƃ��̏Ռ�
        this.rb.AddForce((this.rb.velocity * 0.3f), ForceMode2D.Impulse);
    }

    /**
     * @brief 	�X�V����
    */
    public override void Update()
    {
        Debug.Log("���̒�");
        // ���E�ړ�����
        float moveInput = Input.GetAxis("Horizontal");
        // ���͒l�̃f�b�h�]�[����K�p
        if (Mathf.Abs(moveInput) < this.inputDeadZone) { moveInput = 0.0f; }

        // �ړ���
        if (moveInput != 0.0f)
        {
            this.Move(moveInput);
        }
        // �~�܂�����
        else
        {
            if (this.rb.velocity.normalized.x != 0.0f)
            {
                // ����������
                this.rb.AddForce(new Vector2(-(this.rb.velocity.x * this.moveDamping * this.moveFactor), 0.0f), ForceMode2D.Impulse);
            }

            // �����h������S�Ɏ~�܂�����u�~�܂����v
            this.isStopped = Stopped();
        }

        // ���x���v�Z
        float speed = moveInput * moveSpeed * this.moveFactor;
        this.rb.AddForce(new Vector2(speed, 0.0f), ForceMode2D.Force);

        // ���̔�����
        this.rb.AddForce(-(this.rb.velocity * (1.0f - this.moveFactor)), ForceMode2D.Force);
    }

    /**
     * @brief 	�����������̏���
     *  @param  Collider2D _collision    ���������I�u�W�F�N�g
    */
    public override void CollisionEnter(Collider2D _collision)
    { }
}
