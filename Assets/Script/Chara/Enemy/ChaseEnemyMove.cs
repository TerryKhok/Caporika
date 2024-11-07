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

    private float rayDistance = 10f;

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

     void Update()
    {
        Vector2 origin = transform.position;
        int layerMask = ~(1 << 10); // ���C���[8�iHunterLayer�j�𖳎�����}�X�N

        // �������Ray���΂�
        RaycastHit2D hitUp = Physics2D.Raycast(origin, Vector2.up, rayDistance, layerMask);
        if (hitUp.collider != null)
        {

            if (hitUp.collider.tag == "Player")
            {
                Debug.Log("�������Player�Ƀq�b�g");

                PlayerMove playerMove = hitUp.collider.GetComponent<PlayerMove>();
                Debug.Log(playerMove);
                if (playerMove != null && playerMove.playerCondition != PlayerState.PlayerCondition.Dead)
                {
                    this.isAttacking = true;

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
            else if (hitUp.collider.tag == "TriggerArea")
            {
                Transform parentTransform = hitUp.collider.transform.parent;

                // �e�I�u�W�F�N�g�����݂���ꍇ
                if (parentTransform != null)
                {

                    PlayerMove playerMove = parentTransform.GetComponent<PlayerMove>();
                    Debug.Log(playerMove);
                    if (playerMove != null && playerMove.playerCondition != PlayerState.PlayerCondition.Dead)
                    {
                        this.isAttacking = true;

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
            else
            {
                //Debug.Log("�����: " + hitUp.collider.tag);
            }
        }

        // ��������Ray���΂�
        RaycastHit2D hitDown = Physics2D.Raycast(origin, Vector2.down, rayDistance, layerMask);
        if (hitDown.collider != null)
        {
            if (hitDown.collider.tag == "Player")
            {
                Debug.Log("��������Player�Ƀq�b�g");

                PlayerMove playerMove = hitDown.collider.GetComponent<PlayerMove>();
                Debug.Log(playerMove);
                if (playerMove != null && playerMove.playerCondition != PlayerState.PlayerCondition.Dead)
                {
                    this.isAttacking = true;

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
            else if (hitDown.collider.tag == "TriggerArea")
            {
                Transform parentTransform = hitDown.collider.transform.parent;
                Debug.Log("�e :" + parentTransform);

                // �e�I�u�W�F�N�g�����݂���ꍇ
                if (parentTransform != null)
                {

                    PlayerMove playerMove = parentTransform.GetComponent<PlayerMove>();
                    Debug.Log(playerMove);
                    if (playerMove != null && playerMove.playerCondition != PlayerState.PlayerCondition.Dead)
                    {
                        this.isAttacking = true;

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
                else
                {
                    Debug.Log("������:" + hitDown.collider.tag);
                }
            }
        }
    }


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
    //public void OnChildTriggerStay2D(Collider2D _other)
    //{
    //    // �}�g�����[�V�J�̌��ɗ����Ƃ�
    //    if (_other.CompareTag("Player"))
    //    {
    //        // �}�g�����[�V�J������ł��Ȃ����
    //        PlayerMove playerMove = _other.GetComponent<PlayerMove>();
    //        if(playerMove.playerCondition!=PlayerState.PlayerCondition.Dead)
    //        {
    //            // �u�U�����s���v
    //            this.isAttacking = true;

    //            // �v���C���[���Q�[���I�[�o�[�ɂ���
    //            MatryoshkaManager playerManager = FindAnyObjectByType<MatryoshkaManager>();
    //            if (!playerManager)
    //            {
    //                Debug.LogError("MatryoshkaManager���擾�ł��܂���ł����B");
    //                return;
    //            }
    //            int currentLife = playerManager.GetCurrentLife();
    //            playerMove.ChangePlayerCondition(PlayerState.PlayerCondition.Dead);
    //            playerManager.GameOver();
    //        }
    //    }
    //}

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
