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
 *          �E�U��(������)
*/
public class PlayerAction : MonoBehaviour
{
    public float jumpForce = 10.0f;                     // �͂̑傫��
    public string tagName;                              // �}�g�����[�V�J�̐e�^�O��

    private bool isSametag = false;                     // ����Tag��
    private Collider2D currentTrigger = null;           // ���������Ă���I�u�W�F�N�g
    private CharaState triggerState = null;             // �������Ă���I�u�W�F�N�g�̏��

    private CharaState matryoishkaState = null;         // ���̏��
    private MatryoshkaManager matryoshkaManager = null;
    private int sizeState = 0;                          // ���g�̃T�C�Y

    private void Start()
    {
        matryoishkaState = GetComponent<CharaState>();              // ���̃}�g�����[�V�J�̏��
        sizeState = matryoishkaState.GetCharaSize();                // ���̃}�g�����[�V�J�̑傫��
        matryoshkaManager = FindAnyObjectByType<MatryoshkaManager>();   
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
                matryoishkaState.SetCharaState(CharaState.State.Dead);
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
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // �^�O���������Ƃ��A���������I�u�W�F�N�g��ێ�
        if (other.CompareTag(tagName))
        {
            isSametag = true;
            currentTrigger = other;
            triggerState = other.gameObject.GetComponentInParent<CharaState>(); // �}�g�����[�V�J�̏��
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
    }

    /**
     * @brief 	�}�g�����[�V�J�̒��ɓ��鏈��
    */
    void NestInside()
    {
        // �e�̃}�g�����[�V�J�̃X�N���v�g���擾
        PlayerMove triggerMove = triggerState.GetComponentInParent<PlayerMove>();
        PlayerAction triggerAction = triggerState.GetComponentInParent<PlayerAction>();

        // ����}�g�����[�V�J�ɃA�^�b�`����Ă���X�N���v�g��S�ėL����
        if (triggerMove != null) triggerMove.enabled = true;
        if (triggerAction != null) triggerAction.enabled = true;

        // ����}�g�����[�V�J�̏�Ԃ��u�ʏ�v�ɂ���
        triggerState.GetComponentInParent<CharaState>().SetCharaState(CharaState.State.Normal);

        // ���̃I�u�W�F�N�g������
        Destroy(gameObject);
    }

    /**
     * @brief 	�}�g�����[�V�J�����яo�鏈��
    */
    void PopOut()
    {
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
            rb.isKinematic = false; // �������Z��L����
        }

        // �p�x�����W�A���ɕϊ�
        float eulerAngleZ = rotation.eulerAngles.z;
        float angleInRadians = eulerAngleZ * Mathf.Deg2Rad;
        Vector2 forceDirection = Vector2.zero;

        // �X��������Ƃ��̌v�Z
        if (angleInRadians != 0)
        {
            // �p�x�Ɋ�Â��x�N�g���̒���
            float angle = 0.0f;
            if (eulerAngleZ >= 0 && eulerAngleZ <= 90)
            {
                // ���ɌX���Ă���ꍇ
                angle = 90 - eulerAngleZ;
            }
            else if (eulerAngleZ >= 270 && eulerAngleZ <= 360)
            {
                // �E�ɌX���Ă���ꍇ
                angle = eulerAngleZ - 270;
            }

            // �΂ߕ����̃x�N�g�����v�Z
            forceDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            // �x�N�g���𐳋K�����Ē���
            forceDirection.Normalize();
            forceDirection.x *= 0.5f;
            forceDirection.y *= 0.5f;

            // �p�x�Ɋ�Â��x�N�g���̒���
            if (eulerAngleZ >= 0 && eulerAngleZ <= 90)
            {
                // ���ɌX���Ă���ꍇ
                forceDirection.x = -Mathf.Abs(forceDirection.x); // x�����}�C�i�X
                forceDirection.y = Mathf.Abs(forceDirection.y);  // y�����v���X
            }
            else if (eulerAngleZ >= 270 && eulerAngleZ <= 360)
            {
                // �E�ɌX���Ă���ꍇ
                forceDirection.x = Mathf.Abs(forceDirection.x);  // x�����v���X
                forceDirection.y = Mathf.Abs(forceDirection.y);  // y�����v���X
            }
        }    
        else
        {
            // 0�x�̎��͂܂�������΂�
            forceDirection = Vector2.up * 0.5f;
        }

        //// �͂̕���
        //Debug.Log("Force Direction After Adjustment: " + forceDirection);
        //Debug.Log("Velocity before AddForce: " + rb.velocity);

        // �͂�������
        rb.AddForce(forceDirection * jumpForce, ForceMode2D.Impulse);
        //Debug.Log("Velocity after AddForce: " + rb.velocity);

        // ��яo���}�g�����[�V�J���u��񂾁v��Ԃ�
        CharaState newState = newobj.GetComponent<CharaState>();
        if (newState != null)
        {
            newState.SetCharaState(CharaState.State.Flying);
        }
        else{ Debug.LogError("newState��null"); }

        // �ړ��X�N���v�g�𖳌���
        PlayerMove moveScript = GetComponent<PlayerMove>();
        if (moveScript != null)
        {
            // �������~�߂�
            Rigidbody2D moveRb = moveScript.GetComponent<Rigidbody2D>();
            if (moveRb != null)
            {
                moveRb.velocity = Vector2.zero;
            }
        }

        // ���̃X�N���v�g�𖳌���
        enabled = false;
    }
}