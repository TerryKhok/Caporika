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
    private Vector3 origineScale = Vector3.zero;                        // ���̃{�^���̑傫��

    // Start is called before the first frame update
    void Start()
    {
        // ���̑傫����ێ�
        origineScale = transform.localScale;
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
                if(!isTriggerEventRelease)
                {
                    // �{�^���������ꂽ�Ƃ��̃C�x���g���Ăяo��
                    onReleased.Invoke();
                    isTriggerEventRelease=true;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �{�^����G���Ă����I�u�W�F�N�g�����X�g�ɒǉ�
        isTouched = true;
        touchedObjects.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // �{�^����G���Ă����I�u�W�F�N�g�����X�g����폜
        touchedObjects.Remove(collision.gameObject);
        if(touchedObjects.Count <= 0)
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
        float newScaleY = transform.localScale.y - pressSpeed;
        if (newScaleY < 0.3f)
        {
            newScaleY = 0.3f;
            isPressedButton = true;
        }
        transform.localScale = new Vector3(transform.localScale.x, newScaleY, transform.localScale.z);
        return isPressedButton;
    }

    // �{�^���𗣂�����
    private bool ReleasedButton()
    {
        bool isReleasedButton = false;

        // �{�^����L�΂��Č��ɖ߂�
        float newScaleY = transform.localScale.y + pressSpeed;
        if (newScaleY > origineScale.y) 
        { 
            newScaleY = origineScale.y;
            isReleasedButton = true;
        }
        transform.localScale = new Vector3(transform.localScale.x, newScaleY, transform.localScale.z);
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
                totalSize += charaState.GetCharaSize();
            }
        }

        // �{�^�������d���Ƃ�������
        if (totalSize >= buttonMass) { return true; }
        return false;
    }
}