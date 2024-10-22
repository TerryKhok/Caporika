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
    private PreventBounce preventBounce = null;                                     // ���˖h�~�X�N���v�g
    private Animator animator = null;                                               // �v���C���[�̃A�j���[�^�[

    private float blinkTimeCount = 0.0f;    // �u��������Ԋu���Ԃ��J�E���g
    private float sleepTimeCount = 0.0f;    // �Q��Ԋu���Ԃ��J�E���g

    private float blinkCount = 5.0f;        // �u������Ԋu

    private float sleepCount = 10.0f;       // ����Ԋu
    private bool isSleep = false;           // true:�Q��


    /**
     * @brief 	���̏�Ԃɓ���Ƃ��ɍs���֐�
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

        // ���˖h�~�X�N���v�g��L����
        this.preventBounce = _playerMove.GetComponent<PreventBounce>();
        if (!this.preventBounce)
        {
            Debug.LogError("PreventBounce���擾�ł��܂���ł����B");
            return;
        }

        // ���˖h�~�X�N���v�g��L����
        this.preventBounce.enabled = true;
        Debug.Log("���˖h�~�L��");

        // �v���C���[�̃A�j���[�^�[
        this.animator = _playerMove.GetComponent<Animator>();
        if (!this.animator)
        {
            Debug.LogError("Animator���擾�ł��܂���ł����B");
            return;
        }

        // �U�������A�j���[�V����
        if (_playerMove.attackState == PlayerState.AttackState.Success)
        {
            this.animator.SetTrigger("hitTrigger");
        }

        // �U���s�����A�j���[�V����
        else if (_playerMove.attackState == PlayerState.AttackState.Failed)
        {
            this.animator.SetTrigger("nonHitTrigger");
        }

        // �U����ԃ��Z�b�g
        _playerMove.SetAttackState(PlayerState.AttackState.None);
    }

    /**
     * @brief 	���̏�Ԃ���o��Ƃ��ɍs���֐�
    */
    public override void Exit() 
    {
        // ���˖h�~�X�N���v�g�𖳌���
        if (this.preventBounce.enabled) { this.preventBounce.enabled = false; }
        Debug.Log("���˖h�~����");
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

        // �ړ���
        if (moveInput != 0.0f)
        {
            this.Move(moveInput);

            // �ʏ�A�j���[�V�����̂Ƃ��J�E���g���Z�b�g
            if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
               // Debug.Log("�ʏ�A�j���[�V������");

                this.blinkTimeCount = 0.0f;
                this.sleepTimeCount = 0.0f;
                this.isSleep = false;
            }
        }
        // �~�܂�����
        else
        {
            // �A�j���[�V����
            this.PlayerAnimation();

            // ����������
            if (this.rb.velocity.normalized.x != 0.0f)
            {          
                this.rb.AddForce(new Vector2(-(this.rb.velocity.x* this.moveDamping), 0.0f), ForceMode2D.Impulse);
            }

            // �����h������S�Ɏ~�܂�����u�~�܂����v
            this.isStopped = Stopped();
        }

        // ���x���v�Z
        float speed = moveInput * moveSpeed;
        this.rb.AddForce(new Vector2(speed, 0.0f), ForceMode2D.Force);
    }

    /**
     * @brief 	�����������̏���
     *  @param  Collider2D _collision    ���������I�u�W�F�N�g
    */
    public override void CollisionEnter(Collider2D _collision) { }

    private void PlayerAnimation()
    {
        this.sleepTimeCount += Time.deltaTime;
        this.blinkTimeCount += Time.deltaTime;

        // ����D��
        if (!this.isSleep)
        {
            // ����A�j���[�V����
            if (this.sleepTimeCount > this.sleepCount)
            {
                this.isSleep = false;
                this.animator.SetTrigger("sleepTrigger");

                // �A�j���[�V�����̊Ԋu�����Z�b�g
                this.sleepTimeCount = 0.0f;
                this.blinkTimeCount = 0.0f;
            }
        }

        // �u���A�j���[�V����
        if (!this.isSleep && this.blinkTimeCount > this.blinkCount)
        {
            this.animator.SetTrigger("blinkTrigger");

            // �A�j���[�V�����̊Ԋu�����Z�b�g
            this.blinkTimeCount = 0.0f; 
        }
    }
}
