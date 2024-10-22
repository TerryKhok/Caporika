using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
    public string playerObjname;                    // ���������}�g�����[�V�J��z�u����e�v���C���[�I�u�W�F�N�g��
    private GameObject playerObject = null;         // ���������}�g�����[�V�J��z�u����e�v���C���[�I�u�W�F�N�g
    private GameObject newMatryoshka = null;        // �V�����}�g�����[�V�J

    private Animator animator;              // �v���C���[�̃A�j���[�^�[

    public float jumpForce = 10.0f;         // �͂̑傫��
    public string tagName;                  // �}�g�����[�V�J�̐e�^�O��
    public float knockbackAngle;            // �U�����ꂽ���̃m�b�N�o�b�N����p�x
    public float knockbackpForce;           // �m�b�N�o�b�N����З�

    public float actionTime;                // �ĂуW�����v�ł���܂ł̎���

    private PlayerMove matryoishkaMove = null;              // ���g�̏��
    private MatryoshkaManager matryoshkaManager = null;     // �}�g�����[�V�J�̊Ǘ�
    private int matryoishkaSize = 0;                        // ���g�̃T�C�Y
    private Rigidbody2D matryoshkaRb = null;                // ���g��rigidbody2d

    private bool isJump = false;                            // true:�W�����v�ł���
    private float time = 0.0f;                              // �J�E���g�p

    [Header("�W�����v������p�x(�e�X0.0~37.5�Őݒ肵��)")]
    public float outerAngle;    // �O���̊p�x
    public float innerAngle;    // �����̊p�x

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
        // ���̃}�g�����[�V�J�̓�����
        this.matryoishkaMove = GetComponent<PlayerMove>();                      
        if (!this.matryoishkaMove)
        {
            Debug.LogError("PlayerMove���擾�ł��܂���ł����B");
            return;
        }

        // ���̃}�g�����[�V�J�̑傫��
        this.matryoishkaSize = GetComponent<CharaState>().GetCharaSize();

        // �v���C���[�̊Ǘ�
        this.matryoshkaManager = FindAnyObjectByType<MatryoshkaManager>();     
        if (!this.matryoshkaManager)
        {
            Debug.LogError("MatryoshkaManager���擾�ł��܂���ł����B");
            return;
        }

        // �v���C���[��Rigidbody2D
        this.matryoshkaRb =GetComponent<Rigidbody2D>();                        
        if (!this.matryoshkaRb)
        {
            Debug.LogError("Rigidbody2D���擾�ł��܂���ł����B");
            return;
        }

        // �v���C���[�̃A�j���[�^�[
        this.animator = GetComponent<Animator>();                               
        if (!this.animator)
        {
            Debug.LogError("Animator���擾�ł��܂���ł����B");
            return;
        }

        // �v���C���[
        this.playerObject = GameObject.Find(this.playerObjname);
        if (!this.playerObject)
        {
            Debug.LogError("player�I�u�W�F�N�g�������炸�A�擾�ł��܂���ł����B");
            return;
        }
    }

    private void FixedUpdate()
    {
        // ��莞�Ԕ�ׂȂ��悤�ɂ���
        if (!this.isJump)
        {
            this.time += Time.deltaTime;
        }
        if(this.time>this.actionTime)
        {
            this.isJump = true;
            this.time = 0.0f;
        }
    }

    void Update()
    {
        if(this.isJump)
        {
            // ��яo���A�N�V�������s��----------------------------------------------------
            if (Input.GetKeyDown(this.popOutkey))
            {
                // ��������ԏ������Ȃ�
                if (this.matryoishkaSize > 1)
                {
                    // �J���A�j���[�V����
                    this.animator.SetTrigger("openTrigger");

                    // ��яo������
                    PopOut();
                    this.isJump = false;
                    // ���̃}�g�����[�V�J�̏�Ԃ��u���񂾁v��
                    this.matryoishkaMove.ChangePlayerCondition(PlayerState.PlayerCondition.Dead);
                    // �U���s�����
                    this.newMatryoshka.GetComponent<PlayerMove>().SetAttackState(PlayerState.AttackState.Failed);
                }
                return;
            }
        }

        // �}�g�����[�V�J�ɓ���A�N�V�������s��----------------------------------------------------
        if (Input.GetKeyDown(this.nestInsideKey))
        {
            // �^�O���������ŁA��������1�傫���}�g�����[�V�J�̂Ƃ�
            if (this.isSametag && this.matryoishkaSize + 1 == this.triggerSize)
            {
                // ����A�j���[�V�����H�H

                // ���鏈��
                NestInside();
            }
            return;
        }

        // �G�Ɠ������Ă��āA�G������ł��Ȃ�
        if (this.isCollEnemy && enemyColl.GetComponent<CharaMove>().GetCharaCondition() != CharaMove.CharaCondition.Dead)
        {
            // ���g�����ł���Ƃ�
            if (this.matryoishkaMove.playerCondition == PlayerState.PlayerCondition.Flying)
            {
                // �U������
                Attack();
                // �U���������
                this.matryoishkaMove.SetAttackState(PlayerState.AttackState.Success); 
            }
            else
            {
                // �_���[�W���󂯂�
                Damage();

                // ���g�̏�Ԃ��u���񂾁v��
                this.matryoishkaMove.ChangePlayerCondition(PlayerState.PlayerCondition.Dead);
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
        this.newMatryoshka = this.matryoshkaManager.InstanceMatryoshka(this.matryoishkaSize - 1);
        if (!this.newMatryoshka)
        {
            Debug.LogError("�}�g�����[�V�J�̐����Ɏ��s");
            return;
        }
        // Player�^�u���ɐ��������}�g�����[�V�J��z�u
        this.newMatryoshka.transform.parent = this.playerObject.transform;

       // ��яo���͈͂ɂ���Đ��������}�g�����[�V�J�̔�яo���p�x�����߂�
        Vector3  angle = this.transform.eulerAngles;
        if ((angle.z >= 0.0f && angle.z < 15.0f)|| (angle.z >= 345.0f && angle.z < 360.0f)) 
        { 
            rotation = Quaternion.Euler(angle.x, angle.y, 0.0f);
            //Debug.Log("�^��");
        }
        else if (angle.z >= 15.0f && angle.z < 52.5f)
        {
            rotation = Quaternion.Euler(angle.x, angle.y, (15.0f + this.outerAngle));
            //Debug.Log("������");
        }
        else if (angle.z >= 52.5f && angle.z < 90.0f) 
        { 
            rotation = Quaternion.Euler(angle.x, angle.y, (52.5f + this.innerAngle));
            //Debug.Log("���O��");
        }
        else if (angle.z >= 270.0f && angle.z < 307.5) 
        { 
            rotation = Quaternion.Euler(angle.x, angle.y, (270.0f + this.outerAngle));
            //Debug.Log("�E�O��");
        }
        else if (angle.z >= 307.5 && angle.z < 345.0f) 
        {
            rotation = Quaternion.Euler(angle.x, angle.y, (307.5f+this.innerAngle));
            //Debug.Log("�E����");
        }

        //Debug.Log("������:" + this.transform.eulerAngles);

        // ��]�A���W���Z�b�g
        this.newMatryoshka.transform.position = position;
        this.newMatryoshka.transform.rotation = rotation;

        // ���������}�g�����[�V�J��Rigidbody2D���擾
        Rigidbody2D rb = this.newMatryoshka.GetComponent<Rigidbody2D>();
        if (!rb)
        {
            Debug.LogError("���������}�g�����[�V�J��Rigidbody2D�����݂��Ȃ������̂ŁA�ǉ����܂����B");
            rb = this.newMatryoshka.AddComponent<Rigidbody2D>();
        }

        // �}�g�����[�V�J�̌X���Ă�������ɔ��ł���
        Vector2 jumpDirection = this.newMatryoshka.transform.up * jumpForce;
        rb.AddForce(jumpDirection, ForceMode2D.Impulse);

        // ��яo���}�g�����[�V�J���u��񂾁v��Ԃ�
        PlayerMove newMatoryoshkaMove = this.newMatryoshka.GetComponent<PlayerMove>();
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
        this.newMatryoshka = this.matryoshkaManager.InstanceMatryoshka(this.matryoishkaSize - 1);
        if (this.newMatryoshka == null)
        {
            Debug.LogError("�}�g�����[�V�J�̐����Ɏ��s");
            return;
        }
        // Player�^�u���ɐ��������}�g�����[�V�J��z�u
        this.newMatryoshka.transform.parent = this.playerObject.transform;

        // ���݂̃}�g�����[�V�J�̈ʒu�A�p�x���擾
        Vector2 position = this.transform.position;
        Quaternion rotation = this.transform.rotation;

        // ���������}�g�����[�V�J�����݂̃}�g�����[�V�J�Ɠ�����]�A���W�ɃZ�b�g
        this.newMatryoshka.transform.position = position;
        this.newMatryoshka.transform.rotation = rotation;

        // ���������}�g�����[�V�J��Rigidbody2D���擾
        Rigidbody2D rb = this.newMatryoshka.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("���������}�g�����[�V�J��Rigidbody2D�����݂��Ȃ������̂ŁA�ǉ����܂����B");
            rb = this.newMatryoshka.AddComponent<Rigidbody2D>();
        }

        // �p�x�����W�A���ɕϊ�
        float angleInRadians = moveAngle * Mathf.Deg2Rad;

        // ��яo�������x�N�g�����v�Z����
        Vector2 knockbackDirection = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians)).normalized * this.knockbackpForce;   
        rb.AddForce(knockbackDirection, ForceMode2D.Impulse);

        // ��яo���}�g�����[�V�J�́u��񂾁v��Ԃ�!!

        PlayerMove newMatoryoshkaMove = this.newMatryoshka.GetComponent<PlayerMove>();
        if (newMatoryoshkaMove != null)
        {
            newMatoryoshkaMove.ChangePlayerCondition(PlayerState.PlayerCondition.Flying);
        }
        else { Debug.LogError("���������}�g�����[�V�J��PlayerMove���擾�ł��܂���ł����B"); }
    }

    /**
     * @brief 	�U���������Ƃ��̏���
    */
    void Attack()
    {
        // ���������I�u�W�F�N�g�̏�Ԃ��擾
        CharaMove enemyState = enemyColl.GetComponent<CharaMove>();
        // ��Ԃ��u���񂾁v��
        enemyState.SetCharaCondition(CharaMove.CharaCondition.Dead);

        Debug.Log("�U������");
    }
}