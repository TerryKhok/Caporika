using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief 	�v���C���[���u�_���[�W���󂯂Ă����ԁv�̏������s���N���X
 * 
 *  @memo   �EPlayerState�����N���X�Ɏ���
 *          �E�v���C���[�̏�Ԃ́A��Ɏ�������PlayerMove.cs���ŗ񋓌^(CharaCondition�APlayerCondition)���g�p���Đ؂�ւ���
 *          
 *          �E���̏�Ԃ̎��ړ��͂ł��Ȃ�
 *          �E���̎��G�ɓ������Ă��U������ɂ͂Ȃ�Ȃ�
 *          
 *  ========================================================================================================
 *  
 *          MonoBehaviour���p�����Ȃ����߃p�����[�^�͒��ڂ������Ă��������I�I�I�I�I�I�I�I�I
 *          
 *  ========================================================================================================
*/
public class PlayerStateDamaged : PlayerState
{

    private const float damageFactor = 0.0f;            // �󒆂ɂ��鎞�̓���S�̂ł̗͂̉e���x����(0.0f �` 1.0f)
    private Animator animator = null;                   // �v���C���[�̃A�j���[�^�[

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

        // �v���C���[�̃A�j���[�^�[
        this.animator = _playerMove.GetComponent<Animator>();
        if (!this.animator)
        {
            Debug.LogError("Animator���擾�ł��܂���ł����B");
            return;
        }

        // �v���C���[�̓���̉e���x����
        this.moveFactor = damageFactor;

        // �_���[�W���󂯂�A�j���[�V����
        this.animator.SetTrigger("dmageTrigger");
        Debug.Log("�_���[�W���󂯂�A�j���[�V����");
    }

    /**
     * @brief 	���̏�Ԃ���o��Ƃ��ɍs���֐�
    */
    public override void Exit()
    {
    }

    /**
     * @brief 	�X�V����
    */
    public override void Update()
    {
        // ���E�ړ�����
        float moveInput = Input.GetAxis("Horizontal");
        // ���͒l�̃f�b�h�]�[����K�p
        if (Mathf.Abs(moveInput) < this.inputDeadZone) { moveInput = 0.0f; }

        // ���x���v�Z
        float speed = moveInput * moveSpeed * this.moveFactor;
        this.rb.AddForce(new Vector2(speed, 0.0f), ForceMode2D.Force);
    }

    /**
     * @brief 	�����������̏���
     *  @param  Collider2D _collision    ���������I�u�W�F�N�g
    */
    public override void CollisionEnter(Collider2D _collision)
    {
    }

}
