using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief   ��납��ǂ������Ă���G
 * 
 * @memo    �E �}�g�����[�V�J�̌��ɗ����Ƃ��U���A�j���[�V���������s����
 */
public class ChaseEnemyMove : MonoBehaviour
{
    public Animator animator = null;    // �A�j���[�^�[
    public int layerIndex = 0;          // ��~���������C���[�̃C���f�b�N�X

    public string attackAnimTrigger = "AttackTrigger";  // �U���g���K�[
    public string attackTrigger = "enemyAttack";        // �U�������ԂɂȂ�͈�

    private bool isAttacking = false;                   // true:�U������
    private bool isEndMoveAnimation = false;            // true:����A�j���[�V�������I������

    // Start is called before the first frame update
    void Start()
    {
        // �A�j���[�^�[�̎擾
        if(!this.animator)
        {
            animator = GetComponent<Animator>();
        }
        if (!this.animator)
        {
            Debug.LogError("Animator���擾�ł��܂���ł����B");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    { }

    private void FixedUpdate()
    {
        // �A�j���[�V�������I�����Ă���Γ���A�j���[�V���������s���Ȃ�
        if (this.isEndMoveAnimation) { return; }

        // �U���͈͂ɓ������Ƃ�
        if(this.isAttacking)
        {
            // �ǐՃA�j���[�V�����̏I��
            this.animator.SetTrigger("ChaseEndTrigger");

            // �U���A�j���[�V�����̍Đ�
            this.animator.SetTrigger("AttackTrigger");
            Debug.Log("����A�j���[�V�������C���[�̈ꎞ��~");
            this.isAttacking = false;
        }

        // �U���A�j���[�V�������I��������A����A�j���[�V�������C���[���I������
        if (this.animator.GetCurrentAnimatorStateInfo(this.layerIndex).IsName("Wolf_Walk"))
        {
            this.isEndMoveAnimation = true;
            Debug.Log("����A�j���[�V�������C���[���I��");
        }
    }

    /**
     * @brief 	�q�I�u�W�F�N�g�̃g���K�[������󂯎��֐�
     * @param  Collider2D _other   
     * 
     * memo    ChildTriggerNotifier.cs �ɐe�Ƀg���K�[�����n���������L��
    */
    public void OnChildTriggerEnter2D(Collider2D _other)
    {
        // �}�g�����[�V�J�̌��ɗ����Ƃ�
        if (_other.CompareTag("Player"))
        {
            // �}�g�����[�V�J������ł��Ȃ����
            PlayerMove playerMove = _other.GetComponent<PlayerMove>();
            if(playerMove.playerCondition!=PlayerState.PlayerCondition.Dead)
            {
                // �u�U�����s���v
                this.isAttacking = true;
                Debug.Log("��������������������������");
            }
        }
    }
}
