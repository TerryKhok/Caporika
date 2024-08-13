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
    public float jumpForce = 10.0f;                     // �͂̑傫��
    public string tagName;                              // �}�g�����[�V�J�̐e�^�O��
    public float knockbackAngle;                         // �U�����̃m�b�N�o�b�N����p�x
    public float knockbackpForce;

    private bool isSametag = false;                     // ����Tag��
    private Collider2D currentTrigger = null;           // ���������Ă���I�u�W�F�N�g
    private TenpState triggerState = null;             // �������Ă���I�u�W�F�N�g�̏��

    private TenpState matryoishkaState = null;         // ���g�̏��
    private MatryoshkaManager matryoshkaManager = null; // �}�g�����[�V�J�̊Ǘ�
    private int sizeState = 0;                          // ���g�̃T�C�Y
    private Rigidbody2D matryoshkaRb = null;            // ���g��rigidbody2d

    private bool isCollEnemy = false;                   // �G�ƂԂ�����
    private Collider2D enemyColl=null;                  // �G�̓����蔻��

    private void Start()
    {
        matryoishkaState = GetComponent<TenpState>();              // ���̃}�g�����[�V�J�̏��
        sizeState = matryoishkaState.GetCharaSize();                // ���̃}�g�����[�V�J�̑傫��
        matryoshkaManager = FindAnyObjectByType<MatryoshkaManager>();
        matryoshkaRb=GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ��������ԏ������Ȃ�
            if (sizeState > 1)
            {
                // ��яo������
                PopOut();
                // ���̃}�g�����[�V�J�̏�Ԃ��u���񂾁v��
                matryoishkaState.SetCharaState(TenpState.State.Dead);
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // �^�O���������ŁA��������1�傫���}�g�����[�V�J�̂Ƃ�
            if (isSametag&& sizeState + 1 == triggerState.GetCharaSize())
            {
                // ���鏈��
                NestInside(); 
            }
        }

        // �G�Ɠ������Ă��āA�G������ł��Ȃ����
        if(isCollEnemy&&enemyColl.GetComponent<TenpState>().GetCharaState()!= TenpState.State.Dead)
        {
            // ���g�����ł���Ƃ��Ȃ�
            if (matryoishkaState.state == TenpState.State.Flying)
            {
                // �U������
                Attack();
            }
            else
            {
                // �_���[�W���󂯂�
                Damage();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // �^�O���������Ƃ��A���������I�u�W�F�N�g��ێ�
        if (other.CompareTag(tagName))
        {
            isSametag = true;
            currentTrigger = other;
            triggerState = other.gameObject.GetComponentInParent<TenpState>(); // �}�g�����[�V�J�̏��
        }

        // �G�ɂԂ��������A���������I�u�W�F�N�g��ێ�
        if (other.CompareTag("Enemy"))
        {
            isCollEnemy = true;
            enemyColl = other;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // �����^�O����o���Ƃ��A���Z�b�g
        if (other.CompareTag(tagName))
        {
            isSametag = false;
            currentTrigger = null;
            triggerState = null;
        }        

        if(other.CompareTag("Enemy"))
        {
            isCollEnemy = false;
            enemyColl = null;
        }
    }

    /**
     * @brief 	�}�g�����[�V�J�̒��ɓ��鏈��
    */
    void NestInside()
    {        
        // �c�@�𑝂₷
        matryoshkaManager.AddLife();

        // �e�̃}�g�����[�V�J�̃X�N���v�g���擾
        TempMove triggerMove = triggerState.GetComponentInParent<TempMove>();
        PlayerAction triggerAction = triggerState.GetComponentInParent<PlayerAction>();

        // ����}�g�����[�V�J�ɃA�^�b�`����Ă���X�N���v�g��S�ėL����
        if (triggerMove != null) triggerMove.enabled = true;
        if (triggerAction != null) triggerAction.enabled = true;

        // ����}�g�����[�V�J�̏�Ԃ��u�ʏ�v�ɂ���
        triggerState.GetComponentInParent<TenpState>().SetCharaState(TenpState.State.Normal);

        // ���̃I�u�W�F�N�g������
        Destroy(gameObject);
    }

    /**
     * @brief 	�}�g�����[�V�J�����яo�鏈��
    */
    void PopOut()
    {
        // �c�@�����炷
        int curLife = matryoshkaManager.LoseLife();
        if (curLife <= 0) { return; }

        // ���݂̃}�g�����[�V�J�̈ʒu�A�p�x���擾
        Vector2 position = transform.position;
        Quaternion rotation = transform.rotation;

        // ���g����i�K�������}�g�����[�V�J�𐶐�
        GameObject newobj = matryoshkaManager.InstanceMatryoshka(sizeState - 1);
        if (newobj == null)
        {
            Debug.LogError("�}�g�����[�V�J�̐����Ɏ��s");
            return;
        }

        // �O���̃}�g�����[�V�J�Ɠ�����]�A���W�ɃZ�b�g
        newobj.transform.position = position;
        newobj.transform.rotation = rotation;

        // ���������}�g�����[�V�J��Rigidbody2D���擾
        Rigidbody2D rb = newobj.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = newobj.AddComponent<Rigidbody2D>();
            rb.isKinematic = false; 
        }

        // �I�u�W�F�N�g�̌X���������ɔ��ł���
        Vector2 jumpDirection = transform.up * jumpForce;
        rb.AddForce(jumpDirection, ForceMode2D.Impulse);

        // ��яo���}�g�����[�V�J���u��񂾁v��Ԃ�
        TenpState newState = newobj.GetComponent<TenpState>();
        if (newState != null)
        {
            newState.SetCharaState(TenpState.State.Flying);
        }
        else{ Debug.LogError("newState��null"); }

        // ���g�̏�Ԃ��u���񂾁v��
        matryoishkaState.SetCharaState(TenpState.State.Dead);
        matryoshkaRb.velocity = Vector2.zero;

        // ���̃X�N���v�g�𖳌���
        enabled = false;
    }

    /**
     * @brief 	�U�����󂯂��Ƃ��̏���
     * 
     * @memo     �U�����󂯂����Ƃ͔��Ε����ɒ��g���΂�
    */
    void Damage()
    {
        // �c�@�����炷
        int curLife = matryoshkaManager.LoseLife();
        if (curLife <= 0){ return; }

        // �U�����󂯂����������яo��p�x������
        float direction = transform.position.x - enemyColl.gameObject.transform.position.x;
        float moveAngle = 0.0f;
        float angle = 0.0f;

        // ������
        if (direction < 0) 
        { 
            moveAngle = 180.0f - knockbackAngle;
            angle = knockbackAngle;
        }
        // �E����
        else
        { 
            moveAngle = knockbackAngle;
            angle = 360.0f - knockbackAngle;
        }

        // �p�x��ݒ�
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, angle);

        // ���g����i�K�������}�g�����[�V�J�𐶐�
        if (sizeState <= 0) { return; }
        GameObject newobj = matryoshkaManager.InstanceMatryoshka(sizeState - 1);
        if (newobj == null)
        {
            Debug.LogError("�}�g�����[�V�J�̐����Ɏ��s");
            return;
        }

        // ���݂̃}�g�����[�V�J�̈ʒu�A�p�x���擾
        Vector2 position = transform.position;
        Quaternion rotation = transform.rotation;

        // ���������}�g�����[�V�J�����݂̃}�g�����[�V�J�Ɠ�����]�A���W�ɃZ�b�g
        newobj.transform.position = position;
        newobj.transform.rotation = rotation;

        // ���������}�g�����[�V�J��Rigidbody2D���擾
        Rigidbody2D rb = newobj.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = newobj.AddComponent<Rigidbody2D>();
            rb.isKinematic = false; // �������Z��L����
        }

        // �p�x�����W�A���ɕϊ�
        float angleInRadians = moveAngle * Mathf.Deg2Rad;

        // ��яo�������x�N�g�����v�Z����
        Vector2 knockbackDirection = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians)).normalized * knockbackpForce;   
        rb.AddForce(knockbackDirection, ForceMode2D.Impulse);

        // ��яo���}�g�����[�V�J�́u�_���[�W�v��Ԃ�
        TenpState newState = newobj.GetComponent<TenpState>();
        if (newState != null)
        {
            newState.SetCharaState(TenpState.State.Damaged);
        }
        else { Debug.LogError("newState��null"); }

        // ���g�̏�Ԃ��u���񂾁v��
        matryoishkaState.SetCharaState(TenpState.State.Dead);
        matryoshkaRb.velocity = Vector2.zero;

        // ���̃X�N���v�g�𖳌���
        enabled = false;

        Debug.Log("�_���[�W���󂯂��I");
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
        enemyState.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        Debug.Log("�U������");
    }
}