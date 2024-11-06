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

    private int stepSoundTimer = 0;     // ������炷���߂̃^�C�}�[

    // Start is called before the first frame update
    void Start()
    {
        // �A�j���[�^�[�̎擾
        if (!this.animator)
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
        if (this.isAttacking)
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

        stepSoundTimer++;
        if (stepSoundTimer > 20)
        {
            stepSoundTimer = 0;
            List<string> soundList = new List<string>
            {
                "ENEMY_STEP_1","ENEMY_STEP_2",
                "ENEMY_STEP_3","ENEMY_STEP_4",
                "ENEMY_STEP_5","ENEMY_STEP_6",
                "ENEMY_STEP_7","ENEMY_STEP_8",
            };
            SoundManager.Instance.PlayRandomSE(soundList);
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
            if (playerMove.playerCondition != PlayerState.PlayerCondition.Dead)
            {
                // �u�U�����s���v
                this.isAttacking = true;

                List<string> soundList = new List<string> { "ENEMY_ROAR_1", "ENEMY_ROAR_2" };
                SoundManager.Instance.PlayRandomSE(soundList);

                // �v���C���[���Q�[���I�[�o�[�ɂ���
                MatryoshkaManager playerManager = FindAnyObjectByType<MatryoshkaManager>();
                if (!playerManager)
                {
                    Debug.LogError("MatryoshkaManager���擾�ł��܂���ł����B");
                    return;
                }
                int currentLife = playerManager.GetCurrentLife();
                playerMove.ChangePlayerCondition(PlayerState.PlayerCondition.Dead);
                playerManager.GameOver();
            }
        }
    }

    /**
     * @brief 	�U�����������擾����֐�
     * @return  bool   this.isAttacking
     * 
     * memo    �v���C���[���ōU�����ꂽ���Ɏg��
    */
    public bool GetIsAttacking()
    {
        return this.isAttacking;
    }
}
