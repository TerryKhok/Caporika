using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/**
 * @brief �C�x���g�𔭐�������g���K�[
 * 
 * @memo �EPlayer��TriggerEnter2D�ŃC�x���g����������
 */
[RequireComponent(typeof(Collider2D))]
public class GimmickEventTrigger : MonoBehaviour
{
    public bool triggerOnce = false;        ///< true����1�񂵂��������Ȃ�
    public UnityEvent onTriggerEnterEvent;  ///< �g���K�[�ɐG�ꂽ�Ƃ��Ɏ��s����C�x���g
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // �v���C���[�ɑ΂��ăg���K�[�𔭓�
        {
            if (this.onTriggerEnterEvent != null)
            {
                if (!this.triggered || !this.triggerOnce)    // 1����������Ă��Ȃ���1��݂̂Ǝw�肳��Ă��Ȃ��Ȃ�
                {
                    this.onTriggerEnterEvent.Invoke();  // �C�x���g�����s
                    triggered = true;
                }
            }
        }
    }
}
