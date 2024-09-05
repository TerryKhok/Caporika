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
    public UnityEvent onTriggerEnterEvent;  ///< �g���K�[�ɐG�ꂽ�Ƃ��Ɏ��s����C�x���g

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // �v���C���[�ɑ΂��ăg���K�[�𔭓�
        {
            if (this.onTriggerEnterEvent != null)
            {
                this.onTriggerEnterEvent.Invoke();  // �C�x���g�����s
            }
        }
    }
}
