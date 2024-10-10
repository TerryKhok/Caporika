using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief 	�v���C���[���u�S�[�����Ă����ԁv�̏������s���N���X
 * 
 *  @memo   �EPlayerState�����N���X�Ɏ���
 *          �E�v���C���[�̏�Ԃ́A��Ɏ�������PlayerMove.cs���ŗ񋓌^(CharaCondition�APlayerCondition)���g�p���Đ؂�ւ���
 *          
 *          �E�~�܂������̔����A�h��鏈�����s��
 *          
 *  ========================================================================================================
 *  
 *          MonoBehaviour���p�����Ȃ����߃p�����[�^�͒��ڂ������Ă��������I�I�I�I�I�I�I�I�I
 *          
 *  ========================================================================================================
*/

public class PlayerStateGoal : PlayerState
{
    PlayerAction playerAction = null;   // �v���C���[�̍s���X�N���v�g

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
        Debug.Log("�S�[���A�j���[�V�����J�n");
        this.playerAction = _playerMove.GetComponent<PlayerAction>();
        if (!this.playerAction)
        {
            Debug.LogError("PlayerAction���擾�ł��܂���ł����B");
            return;
        }
        // �A�N�V�����X�N���v�g�𖳌���
        this.playerAction.enabled = false;
        this.rb.velocity = Vector3.zero;

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
        if (this.rb.velocity.normalized.x != 0.0f)
        {
            // ����������
            this.rb.AddForce(new Vector2(-(this.rb.velocity.x * this.moveDamping), 0.0f), ForceMode2D.Impulse);

        }

        // �����h������S�Ɏ~�܂�����u�~�܂����v
        this.isStopped = Stopped();
    }

    /**
     * @brief 	�����������̏���
     *  @param  Collider2D _collision    ���������I�u�W�F�N�g
    */
    public override void CollisionEnter(Collider2D _collision)
    { }
}
