using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/**
 * @brief 	�v���C���[���u�}�g�����[�V�J�ɓ��낤�Ƃ��Ă����ԁv�̏������s���N���X
 * 
 *  @memo   �EPlayerState�����N���X�Ɏ���
 *          �E�v���C���[�̏�Ԃ́A��Ɏ�������PlayerMove.cs���ŗ񋓌^(CharaCondition�APlayerCondition)���g�p���Đ؂�ւ���
 *          
 *          �E���̏�Ԃ̎��ړ��͂ł��Ȃ�
 *          �E���̎��G�ɓ������Ă��U������ɂ͂Ȃ�Ȃ�
 *          �E���鏈�����I������uGround�v�ɑJ�ڂ���
 *          
 *  ========================================================================================================
 *  
 *          MonoBehaviour���p�����Ȃ����߃p�����[�^�͒��ڂ������Ă��������I�I�I�I�I�I�I�I�I
 *          
 *  ========================================================================================================
*/
public class PlayerStateNestIn : PlayerState
{
    //private Animator animator = null;       // �v���C���[�̃A�j���[�^�[
    private PlayerMove playerMove = null;   // �v���C���[�̓���
    private MatryoshkaManager matryoshkaManager = null;     // �}�g�����[�V�J�̊Ǘ�
    private Collider2D targetColl = null;        // ����I�u�W�F�N�g�̃R���C�_�[

    //private Transform target;            // �W�����v��̃^�[�Q�b�g
    //private AnimationCurve jumpCurve;    // �W�����v�̍����𐧌䂷��A�j���[�V�����J�[�u
    //private float jumpDuration = 1f;     // �W�����v�̎���

    //private bool isJumping = false;
    //private Vector3 startPosition;
    //private float jumpTime;

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
        this.playerMove = _playerMove;

        this.matryoshkaManager = _playerMove.GetComponent<MatryoshkaManager>();
        if (!this.matryoshkaManager)
        {
            Debug.LogError("Rigidbody2D���擾�ł��܂���ł����B");
            return;
        }
        // �c�@�𑝂₷
        this.matryoshkaManager.AddLife();

        //this.rb = _playerMove.GetComponent<Rigidbody2D>();
        //if (!this.rb)
        //{
        //    Debug.LogError("Rigidbody2D���擾�ł��܂���ł����B");
        //    return;
        //}

        //// �v���C���[�̃A�j���[�^�[
        //this.animator = _playerMove.GetComponent<Animator>();
        //if (!this.animator)
        //{
        //    Debug.LogError("Animator���擾�ł��܂���ł����B");
        //    return;
        //}

        //// 
        //startPosition = _playerMove.transform.position;
        //jumpTime = 0f;
        //isJumping = true;
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
        //// �ړ��͖���

        //if (isJumping)
        //{
        //    jumpTime += Time.deltaTime;
        //    float t = jumpTime / jumpDuration;

        //    // �������珈�����~�߂�
        //    if (t > 1.0f)
        //    {
        //        t = 1.0f;
        //        isJumping = false;
        //    }

        //    // �p���{���O�����v�Z���đΏۂ̃I�u�W�F�N�g�Ɍ������ăW�����v������
        //    Vector3 currentPosition = Vector3.Lerp(startPosition, target.position, t);
        //    currentPosition.y += jumpCurve.Evaluate(t);
        //    this.playerMove.transform.position = currentPosition;
        //}
    }

    /**
     * @brief 	�����������̏���
     *  @param  Collider2D _collision    ���������I�u�W�F�N�g
    */
    public override void CollisionEnter(Collider2D _collision)
    {
        // �����I�u�W�F�N�g�̎��A���̃I�u�W�F�N�g�Ɍ������Ĕ��ł���
        if (_collision.CompareTag("Player"))
        {
            this.targetColl = _collision;
        }
    }
}