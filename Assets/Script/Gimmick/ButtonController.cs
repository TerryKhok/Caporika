using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/**
 *  @brief 	�{�^���̓���
*/
public class ButtonController : MonoBehaviour
{
    public float pressSpeed;        // �{�^�����������܂�鑬��
    public float buttonMass;        // �{�^���̏d��
    public UnityEvent onPressed;    // �{�^���������ꂽ�Ƃ��̃C�x���g
    public UnityEvent onReleased;   // �{�^���������ꂽ�Ƃ��̃C�x���g
    private bool isTriggerEventPress = false;
    private bool isTriggerEventRelease = false;


    private bool isTouched = false;                 // true:�{�^���ɐG����
    private bool isPressed = false;                 // true:�{�^����������
    private bool isReleased = false;                // true:�{�^���𗣂���

    private List<GameObject> touchedObjects = new List<GameObject>();   // �{�^����G���Ă���I�u�W�F�N�g�̃��X�g
    private Transform parentTransform;  // �������Ƃ��̌����ڂ̏����p
    private Vector3 origineScale = Vector3.zero;                        // ���̃{�^���̑傫��


    // Start is called before the first frame update
    void Start()
    {
        // ���̑傫����ێ�
        parentTransform = this.transform.parent;
        origineScale = parentTransform.localScale;
    }

    private void FixedUpdate()
    {
        // �G��ꂽ�Ƃ�
        if (isTouched && touchedObjects.Count > 0)
        {
            // ������Ă��Ȃ����
            if (!isPressed)
            {
                // �{�^���������鏈��
                isPressed = PressedButton();
            }
            // �����ꂽ��
            else
            {
                // �d���𔻒�
                if (!CheckedMass() && touchedObjects.Count > 0)
                {
                    // �{�^���̂ق����d��������{�^���������グ��
                    ReleasedButton();
                }
                else
                {
                    if (!isTriggerEventPress)
                    {
                        // �{�^���������ꂽ�Ƃ��̃C�x���g���Ăяo��
                        onPressed.Invoke();
                        isTriggerEventPress = true;
                    }
                }
            }
        }
        // �{�^�����߂��Ă��Ȃ���Ζ߂�
        else if (!isReleased)
        {
            if (ReleasedButton())
            {
                if (!isTriggerEventRelease)
                {
                    // �{�^���������ꂽ�Ƃ��̃C�x���g���Ăяo��
                    onReleased.Invoke();
                    isTriggerEventRelease = true;
                }
                isReleased = true;
            }
        }
        // �����ꂽ��
        else if (isReleased)
        {
            // �C�x���g��Ԃ����Z�b�g
            isTriggerEventPress = false;
            isTriggerEventRelease = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �{�^����G���Ă����I�u�W�F�N�g�����X�g�ɒǉ�
        isTouched = true;
        touchedObjects.Add(collision.gameObject);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // �{�^����G���Ă����I�u�W�F�N�g�����X�g����폜
        touchedObjects.Remove(collision.gameObject);
        if (touchedObjects.Count <= 0)
        {
            isTouched = false;
        }
        isPressed = false;
        isReleased = false;
    }

    // �{�^������������
    private bool PressedButton()
    {
        bool isPressedButton = false;

        // �{�^�������߂ĉ����������o��
        float newScaleY = parentTransform.localScale.y - pressSpeed;
        if (newScaleY < 0.2f)
        {
            newScaleY = 0.2f;
            isPressedButton = true;
        }
        parentTransform.localScale = new Vector3(parentTransform.localScale.x, newScaleY, parentTransform.localScale.z);
        return isPressedButton;
    }

    // �{�^���𗣂�����
    private bool ReleasedButton()
    {
        bool isReleasedButton = false;

        // �{�^����L�΂��Č��ɖ߂�
        float newScaleY = parentTransform.localScale.y + pressSpeed;
        if (newScaleY > origineScale.y)
        {
            newScaleY = origineScale.y;
            isReleasedButton = true;
        }
        parentTransform.localScale = new Vector3(parentTransform.localScale.x, newScaleY, parentTransform.localScale.z);
        return isReleasedButton;
    }

    // �d���𔻒肷�鏈��
    private bool CheckedMass()
    {
        // �G���Ă���I�u�W�F�N�g�̃T�C�Y�̍��v���v�Z
        int totalSize = 0;
        foreach (GameObject obj in touchedObjects)
        {
            CharaState charaState = obj.GetComponent<CharaState>();
            if (charaState != null)
            {
                totalSize += charaState.GetCharaWeight();
            }
        }

        // �{�^�������d���Ƃ�������
        if (totalSize >= buttonMass) { return true; }
        return false;
    }
}