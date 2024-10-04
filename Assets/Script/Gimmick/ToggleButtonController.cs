using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/**
 * @brief   �g�O���{�^���̐���X�N���v�g
 * 
 * @memo    �EOnCollisionEnter2D�ŃI���I�t��؂�ւ���
 *          �EOn�ɂȂ����Ƃ��̊֐�ButtonEnable()
 *          �EOff�ɂȂ������̊֐�ButtonDisable()
 */
public class ToggleButtonController : MonoBehaviour
{

    public float buttonMass;        // �{�^���̏d��
    private bool touched = false;   // �G�����u�Ԃł��邱�Ƃ������t���O
    private bool isOn = false;      // �{�^���̏�Ԃ�\���ϐ��i�I���Ȃ�true�A�I�t�Ȃ�false�j

    // UnityEvent�^�̃C�x���g���Ǘ�����ϐ�
    public UnityEvent onEvent;
    public UnityEvent offEvent;

    public GimmickEventTrigger eventTrigger;
    public List<GameObject> touchedObjects = new List<GameObject>();   // �{�^����G���Ă���I�u�W�F�N�g�̃��X�g
    private Transform parentTransform;              // �ό`�p�̐e��Transform
    private Vector3 originScale = Vector3.zero;    // ���̃{�^���̑傫��


    private void Start()
    {
        // �e��Transform���擾���āA���̃T�C�Y���L�^
        this.parentTransform = transform.parent;
        this.originScale = this.parentTransform.localScale;
    }

    private void FixedUpdate()
    {
        if (touched)
        {
            // �g�O���̏�Ԃɉ����ăC�x���g�𔭉�
            if (this.isOn)
            {
                ButtonEnable();
            }
            else
            {
                ButtonDisable();
            }
        }
    }

    // 2D�R���C�_�[���Փ˂����ۂɌĂяo�����֐�
    public void Triggered()
    {
        Collider2D collider = this.eventTrigger.GetTriggeredCollider();
        if (collider.CompareTag("Player") || collider.CompareTag("Enemy"))
        {
            this.touchedObjects.Add(collider.gameObject);
            // ���O�ɏ����������ĂȂ��Ă��d�����\���Ȃ�
            if (CheckedMass())
            {
                this.isOn = !this.isOn;
                // �g�O���̏�Ԃɉ����ăC�x���g�𔭉�
                if (this.isOn)
                {
                    ButtonEnable();
                }
                else
                {
                    ButtonDisable();
                }
            }
        }
    }

    public void TriggerExit()
    {
        Collider2D collider = this.eventTrigger.GetTriggeredCollider();
        this.touchedObjects.Remove(collider.gameObject);
    }

    private void ButtonEnable()
    {
        Debug.Log("ToggleButtonEnabled");
        this.onEvent.Invoke();

        // �{�^�������߂ĉ����������o��
        //this.touched = false;   // �������I�������t���O��܂�
        float newScaleY = 0.4f;
        this.parentTransform.localScale = new Vector3(this.parentTransform.localScale.x, newScaleY, this.parentTransform.localScale.z);
    }

    private void ButtonDisable()
    {
        Debug.Log("ToggleButtonDisabled");
        this.offEvent.Invoke();

        // �{�^����L�΂��Č��ɖ߂�
        //this.touched = false;   // �������I�������t���O��܂�
        float newScaleY = this.originScale.y;
        this.parentTransform.localScale = new Vector3(this.parentTransform.localScale.x, newScaleY, this.parentTransform.localScale.z);
    }

    // �d���𔻒肷�鏈��
    private bool CheckedMass()
    {
        // �G���Ă���I�u�W�F�N�g�̃T�C�Y�̍��v���v�Z
        int totalSize = 0;
        foreach (GameObject obj in this.touchedObjects)
        {
            CharaState charaState = obj.GetComponent<CharaState>();
            if (charaState != null)
            {
                totalSize += charaState.GetCharaWeight();
            }
        }

        // �{�^�������d���Ƃ�������
        if (totalSize >= this.buttonMass) { return true; }
        return false;
    }
}
