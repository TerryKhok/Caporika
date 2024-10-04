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
    public UnityEvent onTriggerExitEvent;   ///< �g���K�[���痣�ꂽ�Ƃ��Ɏ��s����C�x���g
    private Collider2D triggeredCollider;   ///< �g���K�[���ꂽ�R���C�_�[
    private bool triggeredEnter = false;    ///< ���łɃg���K�[���ꂽ��
    private bool triggeredExit = false;
    private bool enemyCanTriggered = false; ///< �G�ł��g���K�[�o���邩

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.CompareTag("Player") || (enemyCanTriggered && _other.CompareTag("Enemy")))  // �v���C���[�ɑ΂��ăg���K�[�𔭓�
        {
            if (this.onTriggerEnterEvent != null)
            {
                if (!this.triggeredEnter || !this.triggerOnce)    // 1����������Ă��Ȃ���1��݂̂Ǝw�肳��Ă��Ȃ��Ȃ�
                {
                    triggeredCollider = _other;
                    this.onTriggerEnterEvent.Invoke();  // �C�x���g�����s
                    triggeredEnter = true;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D _other)
    {
        if (_other.CompareTag("Player") || (enemyCanTriggered && _other.CompareTag("Enemy")))  // �v���C���[�ɑ΂��ăg���K�[�𔭓�
        {
            if (this.onTriggerExitEvent != null)
            {
                if (!this.triggeredExit || !this.triggerOnce)    // 1����������Ă��Ȃ���1��݂̂Ǝw�肳��Ă��Ȃ��Ȃ�
                {
                    triggeredCollider = _other;
                    this.onTriggerExitEvent.Invoke();  // �C�x���g�����s
                    triggeredExit = true;
                }
            }
        }
    }

    public Collider2D GetTriggeredCollider() { return triggeredCollider; }
}
