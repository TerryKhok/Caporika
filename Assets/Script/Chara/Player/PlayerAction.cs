using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

/**
 *  @brief 	�L�����̍s�����܂Ƃ߂�
 *  
 *  @memo   �E��яo�鏈��
 *          �E���ɓ��鏈��
 *          �E�U��
*/
public class PlayerAction : MonoBehaviour
{
    //===============================================
    //                  �L�[
    //===============================================
    public KeyCode  popOutkey = KeyCode.Space;          // ��яo���A�N�V�������s��
    public KeyCode nestInsideKey = KeyCode.LeftShift;   // �}�g�����[�V�J�ɓ���A�N�V�������s��


    //===============================================
    //          �}�g�����[�V�J
    //===============================================

    public float jumpForce = 10.0f;         // �͂̑傫��
    public string tagName;                  // �}�g�����[�V�J�̐e�^�O��
    public float knockbackAngle;            // �U�����ꂽ���̃m�b�N�o�b�N����p�x
    public float knockbackpForce;           // �m�b�N�o�b�N����З�

    private PlayerMove matryoishkaMove = null;              // ���g�̏��
    private MatryoshkaManager matryoshkaManager = null;     // �}�g�����[�V�J�̊Ǘ�
    private int matryoishkaSize = 0;                       // ���g�̃T�C�Y
    private Rigidbody2D matryoshkaRb = null;               // ���g��rigidbody2d


    //===============================================
    //          �������Ă���I�u�W�F�N�g
    //===============================================

    private bool isSametag = false;                     // ����Tag��
    private Collider2D currentTrigger = null;           // ���������Ă���I�u�W�F�N�g
    private PlayerMove triggerMove = null;              // �������Ă���I�u�W�F�N�g�̏��
    private int triggerSize = 0;                        // ���������Ă���I�u�W�F�N�g�̃T�C�Y

    private bool isCollEnemy = false;                   // �G�ƂԂ�����
    private Collider2D enemyColl=null;                  // �G�̓����蔻��

    private void Start()
    {
        this.matryoishkaMove = GetComponent<PlayerMove>();                      // ���̃}�g�����[�V�J�̓�����
        this.matryoishkaSize = GetComponent<CharaState>().GetCharaSize();       // ���̃}�g�����[�V�J�̑傫��
        this.matryoshkaManager = FindAnyObjectByType<MatryoshkaManager>();
        this.matryoshkaRb =GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // ��яo���A�N�V�������s��----------------------------------------------------
        if (Input.GetKeyDown(this.popOutkey))
        {
            // ��������ԏ������Ȃ�
            if (this.matryoishkaSize > 1)
            {
                // ��яo������
                PopOut();
                // ���̃}�g�����[�V�J�̏�Ԃ��u���񂾁v��
                this.matryoishkaMove.ChangePlayerCondition(PlayerState.PlayerCondition.Dead);

                // ���̃X�N���v�g�𖳌���
                this.enabled = false;
            }
            return;
        }

        // �}�g�����[�V�J�ɓ���A�N�V�������s��----------------------------------------------------
        if (Input.GetKeyDown(this.nestInsideKey))
        {
            // �^�O���������ŁA��������1�傫���}�g�����[�V�J�̂Ƃ�
            if (this.isSametag && this.matryoishkaSize + 1 == this.triggerSize)
            {
                // ���鏈��
                NestInside();
            }
            return;
        }

        // �G�Ɠ������Ă��āA�G������ł��Ȃ�
        if (this.isCollEnemy && enemyColl.GetComponent<TenpState>().GetCharaState() != TenpState.State.Dead)
        {
            // ���g�����ł���Ƃ�
            if (this.matryoishkaMove.playerCondition == PlayerState.PlayerCondition.Flying)
            {
                // �U������
                Attack();
            }
            else
            {
                // �_���[�W���󂯂�
                Damage();

                // ���g�̏�Ԃ��u���񂾁v��
                this.matryoishkaMove.ChangePlayerCondition(PlayerState.PlayerCondition.Dead);
                // ���̃X�N���v�g�𖳌���
                this.enabled = false;

                Debug.Log("�_���[�W���󂯂��I");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // �^�O���������Ƃ��A���������I�u�W�F�N�g��ێ�
        if (other.CompareTag(tagName))
        {
            this.isSametag = true;
            this.currentTrigger = other;
            this.triggerSize = other.gameObject.GetComponentInParent<CharaState>().GetCharaSize();   // �}�g�����[�V�J�̃T�C�Y
            this.triggerMove = other.gameObject.GetComponentInParent<PlayerMove>();                  // �}�g�����[�V�J�̏��
        }

        // �G�ɂԂ��������A���������G�I�u�W�F�N�g��ێ�
        if (other.CompareTag("Enemy"))
        {
            this.isCollEnemy = true;
            this.enemyColl = other;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // �����^�O����o���Ƃ��A���Z�b�g
        if (other.CompareTag(tagName))
        {
            this.isSametag = false;
            this.currentTrigger = null;
            this.triggerMove = null;
        }        

        // �G�Ɨ��ꂽ�烊�Z�b�g
        if(other.CompareTag("Enemy"))
        {
            this.isCollEnemy = false;
            this.enemyColl = null;
        }
    }

    /**
     * @brief 	�}�g�����[�V�J�̒��ɓ��鏈��
    */
    void NestInside()
    {        
        // �c�@�𑝂₷
        this.matryoshkaManager.AddLife();

        // �e�̃}�g�����[�V�J�̃X�N���v�g���擾
        PlayerAction triggerAction = this.currentTrigger.GetComponentInParent<PlayerAction>();

        // ����}�g�����[�V�J�ɃA�^�b�`����Ă���s���X�N���v�g��L����
        if (triggerAction)
        {
            triggerAction.enabled = true;
            this.triggerMove.ChangePlayerCondition(PlayerState.PlayerCondition.Ground);
        }
        else { Debug.LogError("����}�g�����[�V�J��PlayerAction���A�^�b�`����Ă��܂���"); }


        // ���̃I�u�W�F�N�g������
        Destroy(this.gameObject);
    }

    /**
     * @brief 	�}�g�����[�V�J�����яo�鏈��
    */
    void PopOut()
    {
        // �c�@�����炷
        int curLife = this.matryoshkaManager.LoseLife();
        if (curLife <= 0) { return; }

        // ���݂̃}�g�����[�V�J�̈ʒu�A�p�x���擾
        Vector2 position = this.transform.position;
        Quaternion rotation = this.transform.rotation;

        // ���g����i�K�������}�g�����[�V�J�𐶐�
        GameObject newMatryoshka = this.matryoshkaManager.InstanceMatryoshka(this.matryoishkaSize - 1);
        if (!newMatryoshka)
        {
            Debug.LogError("�}�g�����[�V�J�̐����Ɏ��s");
            return;
        }

        // �O���̃}�g�����[�V�J�Ɠ�����]�A���W�ɃZ�b�g
        newMatryoshka.transform.position = position;
        newMatryoshka.transform.rotation = rotation;

        // ���������}�g�����[�V�J��Rigidbody2D���擾
        Rigidbody2D rb = newMatryoshka.GetComponent<Rigidbody2D>();
        if (!rb)
        {
            Debug.LogError("���������}�g�����[�V�J��Rigidbody2D�����݂��Ȃ������̂ŁA�ǉ����܂����B");
            rb = newMatryoshka.AddComponent<Rigidbody2D>();
        }

        // �I�u�W�F�N�g�̌X���������ɔ��ł���
        Vector2 jumpDirection = this.transform.up * jumpForce;
        rb.AddForce(jumpDirection, ForceMode2D.Impulse);

        // ��яo���}�g�����[�V�J���u��񂾁v��Ԃ�
        PlayerMove newMatoryoshkaMove = newMatryoshka.GetComponent<PlayerMove>();
        if (newMatoryoshkaMove)
        {
            newMatoryoshkaMove.ChangePlayerCondition(PlayerState.PlayerCondition.Flying);
        }
        else{ Debug.LogError("���������}�g�����[�V�J��PlayerMove���擾�ł��܂���ł����B"); }
    }

    /**
     * @brief 	�U�����󂯂��Ƃ��̏���
     * 
     * @memo     �U�����󂯂����Ƃ͔��Ε����ɒ��g���΂�
    */
    void Damage()
    {
        // �c�@�����炷
        int curLife = this.matryoshkaManager.LoseLife();
        if (curLife <= 0){ return; }

        // �U�����󂯂����������яo��p�x������
        float direction = this.transform.position.x - this.enemyColl.gameObject.transform.position.x;
        float moveAngle = 0.0f;
        float angle = 0.0f;

        // ������
        if (direction < 0.0f) 
        { 
            moveAngle = 180.0f - this.knockbackAngle;
            angle = this.knockbackAngle;
        }
        // �E����
        else
        { 
            moveAngle = this.knockbackAngle;
            angle = 360.0f - this.knockbackAngle;
        }

        // �p�x��ݒ�
        this.transform.rotation = Quaternion.Euler(this.transform.eulerAngles.x, this.transform.eulerAngles.y, angle);

        // ���g����i�K�������}�g�����[�V�J�𐶐�
        if (this.matryoishkaSize <= 0) { return; }
        GameObject newMatryoshka = this.matryoshkaManager.InstanceMatryoshka(this.matryoishkaSize - 1);
        if (newMatryoshka == null)
        {
            Debug.LogError("�}�g�����[�V�J�̐����Ɏ��s");
            return;
        }

        // ���݂̃}�g�����[�V�J�̈ʒu�A�p�x���擾
        Vector2 position = this.transform.position;
        Quaternion rotation = this.transform.rotation;

        // ���������}�g�����[�V�J�����݂̃}�g�����[�V�J�Ɠ�����]�A���W�ɃZ�b�g
        newMatryoshka.transform.position = position;
        newMatryoshka.transform.rotation = rotation;

        // ���������}�g�����[�V�J��Rigidbody2D���擾
        Rigidbody2D rb = newMatryoshka.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("���������}�g�����[�V�J��Rigidbody2D�����݂��Ȃ������̂ŁA�ǉ����܂����B");
            rb = newMatryoshka.AddComponent<Rigidbody2D>();
        }

        // �p�x�����W�A���ɕϊ�
        float angleInRadians = moveAngle * Mathf.Deg2Rad;

        // ��яo�������x�N�g�����v�Z����
        Vector2 knockbackDirection = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians)).normalized * this.knockbackpForce;   
        rb.AddForce(knockbackDirection, ForceMode2D.Impulse);

        // ��яo���}�g�����[�V�J�́u�_���[�W�v��Ԃ�
        PlayerMove newMatoryoshkaMove = newMatryoshka.GetComponent<PlayerMove>();
        if (newMatoryoshkaMove != null)
        {
            newMatoryoshkaMove.ChangePlayerCondition(PlayerState.PlayerCondition.Damaged);
        }
        else { Debug.LogError("���������}�g�����[�V�J��PlayerMove���擾�ł��܂���ł����B"); }
    }

    /**
     * @brief 	�U���������Ƃ��̏���
    */
    void Attack()
    {
        // ���������I�u�W�F�N�g�̏�Ԃ��擾
        TenpState enemyState = enemyColl.GetComponent<TenpState>();
        // ��Ԃ��u���񂾁v��
        enemyState.SetCharaState(TenpState.State.Dead);

        Debug.Log("�U������");
    }
}