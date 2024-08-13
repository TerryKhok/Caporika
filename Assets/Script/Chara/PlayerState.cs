using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    //===============================================
    //          �}�g�����[�V�J�̗h��
    //===============================================

    protected float tiltVelocity = 0.0f;                // �X���̑��x
    protected  float tiltAmount = 45.0f;                // ��]�p�̍ő�l
    protected  float damping = 0.8f;                    // �����W��(�ǂ̂��炢����]�p�x�����炵�Ă�����)
    protected  int maxSwimg = 3;                        // ����h��邩 
    protected �@int swingCount = 0;                     // �h��̉񐔂��J�E���g
    protected �@float angleSwingZone = 1.0f;            // �h�ꂽ��������ǂ���
    protected bool isInDeadZone = false;                // �h��̃f�b�h�]�[�����ɂ��邩�ǂ���

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
     *  @param  Collision _collision    ���������I�u�W�F�N�g
    */
    public abstract void CollisionEnter(Collision _collision);

    ///**
    // * @brief 	������Ȃ��Ȃ������̏���
    // *  @param  Collision _collision    ���������I�u�W�F�N�g
    //*/
    //public abstract void CollisionExit(Collision _collision);
}